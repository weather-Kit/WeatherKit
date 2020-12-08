﻿function setCookies(position) {
    document.cookie = 'latitude=' + position.coords.latitude;
    document.cookie = 'longitude=' + position.coords.longitude;
}

if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(setCookies);
}