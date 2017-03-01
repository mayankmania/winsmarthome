var socket = io.connect();

socket.on('connect', function () {
    socket.emit('getDevices', '1');
});

socket.on('deviceListFetched', function (result) {
    $("#myDevices > tbody").empty();
    for (var i = 0; i < result.length; i++) {
        $("table tbody").append("<tr><th scope='row'>" + (i + 1) + "</th><td><img src='css/icons/" + result[i].device + ".png'" + " class='thumbnail' alt='...'></td><td><a href='javascript:void(0);' onclick='manageDevice(" + result[i].deviceId + ")'><img class='thumbnail' src='css/icons/stop.png' id='device" + result[i].deviceId + "'/> </a></td></tr>");
        setStateIcon(result[i]);
    }
});

socket.on('onDeviceUpdated', function (result) {
    setStateIcon(result);
});

function manageDevice(deviceId) {
    socket.emit('performOperation', { raspId: 1, "deviceId": deviceId });
}

function setStateIcon(result) {
    $("#device" + result.deviceId).attr('src', result.status ? 'css/icons/stop.png' : 'css/icons/start.png');
}