<?php
include "config.php";
include "utils.php";

$dbConn = connect($db);

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    // Obtener datos JSON de la solicitud
    $data = json_decode(file_get_contents("php://input"));

    // Validar datos
    if (isset($data->usuario) && isset($data->contrasena)) {
        $usuario = $data->usuario;
        $contrasena = $data->contrasena;

        // Consultar base de datos
        $sql = $dbConn->prepare("SELECT * FROM estudiantes_login WHERE usuario = :usuario AND contrasena = :contrasena");
        $sql->bindValue(':usuario', $usuario);
        $sql->bindValue(':contrasena', $contrasena);
        $sql->execute();

        if ($sql->rowCount() > 0) {
            // Credenciales válidas
            echo json_encode(true);
        } else {
            // Credenciales inválidas
            echo json_encode(false);
        }
    } else {
        echo json_encode(false);
    }
} else {
    echo json_encode(false);
}
?>
