var map;
var marker = false;

function initMap() {

    var centerOfMap = new google.maps.LatLng(51.487987, -0.237269); // St Paul's School
    
    var options = {
        center: centerOfMap,
        zoom: 10
    };

    // extantiate a map object
    map = new google.maps.Map(document.getElementById('map'), options);

    marker = new google.maps.Marker({
        position: centerOfMap,
        map: map,
        title: 'Drag me around',
        draggable: true
    });

    google.maps.event.addListener(marker, 'dragend', function (event) {
        markerLocation();
    });

    markerLocation();
}

function markerLocation() {
    // give the information back to the HTML
    var currentLocation = marker.getPosition();
    document.getElementById("lat").value = currentLocation.lat();
    document.getElementById("lng").value = currentLocation.lng();
}

google.maps.event.addDomListener(window, 'load', initMap);