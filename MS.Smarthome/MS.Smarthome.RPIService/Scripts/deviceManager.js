var connection = $.hubConnection();
var rpiHubProxy = connection.createHubProxy('rpiHub');
rpiHubProxy.on('deviceListFetched', function (result) {
    console.log(result);
    for (var i = 0; i < result.length; i++) {
        $("table tbody").append("<tr><th scope='row'>" + (i + 1) + "</th><td><img src='css/icons/" + result[i].description + ".png'" + " class='thumbnail' alt='...'></td><td><a href='javascript:void(0);' onclick='manageDevice(" + result[i].deviceId + ")'><img class='thumbnail' src='/css/icons/stop.png' id='device" + result[i].deviceId + "'/> </a></td></tr>");
        setStateIcon(result[i]);
    }
});

rpiHubProxy.on('deviceUpdated', function (result) {
    setStateIcon(result);
});

connection.start()
    .done(function () {
        console.log('Now connected, connection ID=' + $.connection.hub.id);
        rpiHubProxy.invoke('getDevices');
    })
    .fail(function () { console.log('Could not Connect!'); });

function manageDevice(deviceId) {
    rpiHubProxy.invoke('PerformOperation', {
        "RaspId": 1,
        "DeviceId": deviceId,
        "OperationType": 1
    }).done(function () {
        console.log('Invocation of PerformOperation succeeded');
    }).fail(function (error) {
        console.log('Invocation of PerformOperation failed. Error: ' + error);
    });
}

function setStateIcon(result) {
    $("#device" + result.deviceId).attr('src', result.status ? '/css/icons/stop.png' : '/css/icons/start.png');
}