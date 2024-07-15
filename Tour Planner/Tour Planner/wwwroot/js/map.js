window.initMap = (startLat, startLng, endLat, endLng, response) => {
    var map = L.map('map').setView([startLat, startLng], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
    }).addTo(map);

    var startPoint = [startLat, startLng];
    var endPoint = [endLat, endLng];

    L.marker(startPoint).addTo(map).bindPopup('Start').openPopup();
    L.marker(endPoint).addTo(map).bindPopup('End');

    var coordinates = response.routes[0].geometry.coordinates.map(coord => [coord[1], coord[0]]);
    var polyline = L.polyline(coordinates, { color: 'blue' }).addTo(map);

    map.fitBounds(polyline.getBounds());
};
