<?php
include "config.php";
include "utils.php";

$dbConn = connect($db);

if ($_SERVER['REQUEST_METHOD'] == 'GET') {
    if (isset($_GET['cod_estudiante'])) {
        // Mostrar un estudiante
        $sql = $dbConn->prepare("SELECT * FROM estudiantes WHERE cod_estudiante=:cod_estudiante");
        $sql->bindValue(':cod_estudiante', $_GET['cod_estudiante']);
        $sql->execute();
        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
        exit();
    } else {
        // Mostrar lista de estudiantes
        $sql = $dbConn->prepare("SELECT * FROM estudiantes");
        $sql->execute();
        $sql->setFetchMode(PDO::FETCH_ASSOC);
        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetchAll());
        exit();
    }
}

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $input = $_POST;
    $sql = "INSERT INTO estudiantes (nombre, apellido, curso, paralelo, nota_final) VALUES (:nombre, :apellido, :curso, :paralelo, :nota_final)";
    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();

    $postCodigo = $dbConn->lastInsertId();
    if ($postCodigo) {
        $input['cod_estudiante'] = $postCodigo;
        header("HTTP/1.1 200 OK");
        echo json_encode($input);
        exit();
    }
}

if ($_SERVER['REQUEST_METHOD'] == 'DELETE') {
    if (isset($_GET['cod_estudiante'])) {
        $codigo = $_GET['cod_estudiante'];
        $statement = $dbConn->prepare("DELETE FROM estudiantes WHERE cod_estudiante=:cod_estudiante");
        $statement->bindValue(':cod_estudiante', $codigo);
        $statement->execute();
        header("HTTP/1.1 200 OK");
        exit();
    } else {
        header("HTTP/1.1 400 Bad Request");
        echo json_encode(['error' => 'Falta el parámetro cod_estudiante']);
        exit();
    }
}

if ($_SERVER['REQUEST_METHOD'] == 'PUT') {
    parse_str(file_get_contents("php://input"), $input);
    if (isset($input['cod_estudiante'])) {
        $postCodigo = $input['cod_estudiante'];
        $fields = getParams($input);

        $sql = "UPDATE estudiantes SET $fields WHERE cod_estudiante=:cod_estudiante";
        $statement = $dbConn->prepare($sql);
        $statement->bindValue(':cod_estudiante', $postCodigo);
        bindAllValues($statement, $input);

        $statement->execute();
        header("HTTP/1.1 200 OK");
        exit();
    } else {
        header("HTTP/1.1 400 Bad Request");
        echo json_encode(['error' => 'Falta el parámetro cod_estudiante']);
        exit();
    }
}
?>
