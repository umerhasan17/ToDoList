function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}

function showPosition(position) {
    document.getElementById("myLatitude").value = position.coords.latitude;
    document.getElementById("myLongitude").value = position.coords.longitude;
}

