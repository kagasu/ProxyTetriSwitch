<?php
require_once('getallheaders.php');

const SERVER_HOST = 'example.com';
const MOVIE_DIRECTORY = 'movies';
const PASSWORD = '302569db917c44a8bfb3b00267cb927a';
const DEBUG = false; // ignore password check

$headers = getallheaders();
if(!DEBUG && (!isset($headers['Password']) || ($headers['Password'] !== PASSWORD)))
{
    http_response_code(403);
    return;
}
?>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
</head>
<body>
    <div>
        <button onclick="location.reload();">Reload</button>
    </div>
    <div>
        <h1>Videos</h1>
        <ul>
            <?php foreach(array_diff(scandir(MOVIE_DIRECTORY), array('..', '.')) as $movie): ?>
            <li><button onclick="changeVideoSource('<?=$movie?>');"><?=$movie?></button></li>
            <?php endforeach; ?>
        </ul>
    </div>

    <div>
        <video id="video">
            <source id="video_source">
        </video>
    </div>

    <script>
    function changeVideoSource(file)
    {
        document.getElementById('video_source').src = 'http://<?=SERVER_HOST?>/<?=MOVIE_DIRECTORY?>/' + file;
        document.getElementById('video').load();
    }
    </script>
</body>
</html>