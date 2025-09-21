var map;

function initMap() {
    var mapDiv = document.getElementById('gllpZoom');
    map = new google.maps.Map(mapDiv, {
        zoom: 8,
        center: {lat: 37.7749, lng: -122.4194}
    });

    var drawingManager = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.POLYGON,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [
                google.maps.drawing.OverlayType.POLYGON
            ]
        },
        polygonOptions: {
            fillColor: '#FF0000',
            fillOpacity: 0.35,
            strokeColor: '#FF0000',
            strokeWeight: 2,
            strokeOpacity: 0.8
        }
    });

    var polygon;

    google.maps.event.addListener(drawingManager, 'overlaycomplete', function (event) {
        if (polygon) {
            if (confirm("A polygon is already drawn on the map. Do you want to remove it and continue?")) {
                polygon.setMap(null);
            } else {
                event.overlay.setMap(null);
                return;
            }
        }
        polygon = event.overlay;
        var path = polygon.getPath();
        var coordinates = [];
        for (var i = 0; i < path.getLength(); i++) {
            var jsonObject = path.getAt(i).toJSON();
            coordinates.push(path.getAt(i).toJSON());
            $(".add__project form").append(`<input type="hidden" name="Map[${i}].lat"  value="${jsonObject.lat}"/>`)
            $(".add__project form").append(`<input type="hidden" name="Map[${i}].lng"  value="${jsonObject.lng}"/>`)
        }
        console.log(coordinates);
    });
    
    drawingManager.setMap(map);
    
    
}

