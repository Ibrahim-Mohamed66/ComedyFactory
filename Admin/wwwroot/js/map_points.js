var map;

var markers = [];
var markerCounter = 0;
//var polygon;
var polygons = [];
var _self = document.getElementById('gllpZoom');
_self.params = {
    defLat: 0,
    defLng: 0,
    defZoom: 14,
    queryLocationNameWhenLatLngChanges: true,
    queryElevationWhenLatLngChanges: true,
    mapOptions: {
        mapTypeControl: false,
        disableDoubleClickZoom: true,
        zoomControlOptions: true,
        streetViewControl: false
    },
    strings: {
        markerText: "Drag this Marker",
        error_empty_field: "Couldn't find coordinates for this place",
        error_no_results: "Couldn't find coordinates for this place"
    },
    displayError: function (message) {
        alert(message);
    }
};
_self.vars = {
    ID: null,
    LATLNG: null,
    map: null,
    marker: null,
    geocoder: null
};
function initMap() {
    var defaultColor ='#FF0000';
    if ("geolocation" in navigator) { 
        navigator.geolocation.getCurrentPosition(function (position) {
            console.log("initMap");

            if ($("#map_shapes_data").length > 0) 
            {
                var shapes = $("#map_shapes_data").val();
                var polygons_data = JSON.parse(shapes);

                $.each(polygons_data, function (i, shape){
                    var polygon_id = shape.Id;
                    var polygon_color = shape.Color ?? defaultColor;
                    var polygonCoords = shape.Coordinates;
                    if (polygonCoords[1] != null) {
                        map = new google.maps.Map(document.getElementById('gllpZoom'), {
                            center: {lat: polygonCoords[1].lat, lng: polygonCoords[1].lng},
                            zoom: _self.params.defZoom
                        });
                    } else {
                        map = new google.maps.Map(document.getElementById('gllpZoom'), {
                            center: {lat: position.coords.latitude, lng: position.coords.longitude},
                            zoom: _self.params.defZoom
                        });
                    }
                    var polygon = new google.maps.Polygon({
                        paths: polygonCoords,
                        strokeColor: polygon_color,
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillColor: polygon_color,
                        fillOpacity: 0.35,
                        editable: true,
                        draggable: true,
                        clickable: true,
                    });
                    polygon.id = polygon_id;
                    get_polygon_click(polygon);
                    polygons.push(polygon)
                });
                if ($("#map_markers_data").length > 0) {
                    var markers_Coordinates = $("#map_markers_data").val();
                    var markersCoords = JSON.parse(markers_Coordinates);
                    if (markersCoords[0] != null) {
                        add_select_markers(markersCoords, 1);
                    }
                }
                

            }
            else if ($("#map_markers_data").length > 0)
            {
                var markers_Coordinates = $("#map_markers_data").val();
                var markersCoords = JSON.parse(markers_Coordinates);
                if (markersCoords[0] != null) 
                {
                    map = new google.maps.Map(document.getElementById('gllpZoom'), {
                        center: {lat: markersCoords[0].lat, lng: markersCoords[0].lng},
                        zoom: _self.params.defZoom
                    });
                    add_select_markers(markersCoords, 2);
                   
                } 
                else
                {
                    map = new google.maps.Map(document.getElementById('gllpZoom'), {
                        center: {lat: position.coords.latitude, lng: position.coords.longitude},
                        zoom: _self.params.defZoom
                    });
                }
              
            }
            else
            {
                map = new google.maps.Map(document.getElementById('gllpZoom'), {
                    center: {lat: position.coords.latitude, lng: position.coords.longitude},
                    zoom: _self.params.defZoom
                });
            }

            manageMapLocation(map);

            
            var drawingManager = new google.maps.drawing.DrawingManager({
                drawingMode: google.maps.drawing.OverlayType.POLYGON,
                drawingControl: false,
                drawingControlOptions: {
                    position: google.maps.ControlPosition.TOP_CENTER,
                    drawingModes: [
                        google.maps.drawing.OverlayType.POLYGON
                    ]
                },
                polygonOptions: {
                    editable: true,
                    draggable: true,
                    fillColor: defaultColor,
                    fillOpacity: 0.35,
                    strokeColor: defaultColor,
                    clickable: true,
                    strokeWeight: 2,
                    strokeOpacity: 0.8
                }
            });
          
            drawingManager.setMap(map);

            google.maps.event.addListener(drawingManager, 'overlaycomplete', function (event) {
               var polygon = event.overlay;
                polygon.type = event.type;
                polygon.id = `polygon_${(polygons.length + 1)}`;
                get_polygon_click(polygon)
                polygons.push(polygon);
                
                getCoordinates(2, polygon, $("#zone_form_map").attr("action"))
            
                google.maps.event.addListener(polygon, 'dragend', function () {
                    getCoordinates(3, polygon, $("#zone_form_map").attr("action"));
                });
            });
             
            
            if(polygons != null && polygons.length > 0){
                $.each(polygons,function (i, _shape){
                    _shape.setMap(map);
                    google.maps.event.addListener(_shape, 'dragend', function () {
                        getCoordinates(4, _shape, $("#zone_form_map").attr("action"));
                    });
                });
            }
            
            google.maps.event.addListener(map, 'click', function (event) {
                UpdateMarkerCoordinates(null,event)
            });

            document.getElementById('draw-shape-button').addEventListener('click', function () {
                drawingManager.setDrawingMode('polygon');
                $(this).removeClass("btn-dark");
                $(this).addClass("btn-primary");
                $("#stop-draw-button").addClass("btn-dark");
            });
            
            document.getElementById('stop-draw-button').addEventListener('click', function () {
                drawingManager.setDrawingMode(null);
                $(this).removeClass("btn-dark");
                $(this).addClass("btn-primary");
                $("#draw-shape-button").addClass("btn-dark");
            });
           
        });
    } else {
        console.log("Browser doesn't support geolocation!");
    }
}
function add_select_markers($markersCoords, $id) {
    $.each($markersCoords, function (i, markerInfo) {
        var location = new google.maps.LatLng(markerInfo.lat, markerInfo.lng);
        var marker = new google.maps.Marker({
            position: location,
            map: map,
            draggable: true,
            id: markerInfo.Id,
            title: markerInfo.EnName,
        });
        markers.push(marker);
        _updateMarker(marker, markerInfo.EnName ?? markerInfo.ArName , markerInfo.Icon);
        google.maps.event.addListener(marker, 'dragend', function (event) {
            UpdateMarkerCoordinates(marker, event)
        });
    })
}

function get_polygon_click($polygon) {
    google.maps.event.clearListeners($polygon, 'click');
    shape_setting($polygon.id, $polygon)
}

function update_shape_setting($id){
    $.each(polygons, function (i, polygon) {
        if (polygon != null && polygon.id == $id) {
            $("#form_shep_setting").html("")

            $.ajax({
                type: "POST",
                url: "/Zones/GetMapShape/" + $id,
                success: function (data) {
                        $("#form_shep_setting").html(data);
                        shape_setting($('#Id_Shep').val(), polygon);
                        $('#Background').on('input', function () {
                            var value = $(this).val();
                            $("#hexcolor_Background").val(value);
                            if (this.value.startsWith("#")) {
                                $(this).val(this.value);
                            }
                        });
                        $('#hexcolor_Background').on('input', function () {
                        $('#Background').val(this.value);
                    });
                        $("#btn_save_shep_setting").off('click');
                        $("#remove_shep").off('click');
                        $("#btn_save_shep_setting").click(function () {
                            var color = $("#Background").val();
                            if (color != null && color != "" && color != " ") {
                                polygon.setOptions({
                                    strokeColor: color,
                                    fillColor: color
                                });
                                getCoordinates(1, polygon, "/Zones/UpdateShapeColorOnMap");
                                $("#zone_shep_setting").modal("hide");

                            }
                        });
                   
                    $("#remove_shep").click(function () {
                        if (confirm("Do you want to remove polygon?")) {
                            $.ajax({
                                url: "/Zones/RemoveShep/" + Number($id),
                                type: 'POST',
                                success: function (response) {
                                    $("#zone_shep_setting").modal("hide")
                                    if (response == "success") {
                                        google.maps.event.clearListeners($polygon, 'click');
                                        $polygon.setMap(null);
                                        $polygon = null;
                                    }
                                },
                                error: function (xhr) {
                                }
                            });
                        } else {
                            return;
                        }
                    });
                }
            });
            return;
        }
    });
}
function remove_shape($id){
    $.each(polygons, function (i, polygon) {
        if (polygon.id == $id) {
            if (confirm("Do you want to remove polygon?")) {
                $.ajax({
                    url: "/Zones/RemoveShep/" + Number($id),
                    type: 'POST',
                    success: function (response) {
                        if (response == "success") {
                            google.maps.event.clearListeners(polygon, 'click');
                            polygon.setMap(null);
                            polygon = null;
                            $('#dataTables_shapes').DataTable().ajax.reload();

                        }
                    },
                    error: function (xhr) {
                    }
                });
            } else {
                return;
            }        }
    });

}

function shape_setting($id,$polygon){
    google.maps.event.clearListeners($polygon, 'click');
    google.maps.event.addListener($polygon, 'click', function () {
        $("#zone_shep_setting").modal("show");
        update_shape_setting($polygon.id);
    });
    
   
}

function updateMapMarker($markerId, $name, $icon) {
    $.each(markers, function (i , marker) {
        if(marker.id == $markerId){
            _updateMarker(marker, $name, $icon)
        }
    });
}

function _updateMarker($marker, $name, $icon) {
    if ($name != null && $name != "") {
        $marker.setOptions({ title: $name});

        if($marker.infoWindow == null){
            google.maps.event.addListener($marker, 'click', function (){
                if ($marker.getTitle() != null && $marker.getTitle() != "") {
                    var infoWindow = new google.maps.InfoWindow({
                        content: $marker.getTitle()
                    });
                    if($marker.infoWindow != null){
                        $marker.infoWindow.close();
                        $marker.infoWindow = null;
                    }else{
                        $marker.infoWindow = infoWindow;
                        infoWindow.open(map, $marker);
                    }

                }else{
                    // Close the infoWindow of the marker.
                    $marker.infoWindow.close();

                    // Remove the infoWindow from the marker.
                    $marker.infoWindow = null;

                    // Remove the click listener from the marker.
                    google.maps.event.clearListeners($marker, 'click');
                }
            });
        }else{

            if ($marker.getTitle() != null && $marker.getTitle() != "") {
                $marker.infoWindow.setContent($marker.getTitle())
            }else{
                if($marker.infoWindow != null){
                    // Close the infoWindow of the marker.
                    $marker.infoWindow.close();

                    // Remove the infoWindow from the marker.
                    $marker.infoWindow = null;

                    // Remove the click listener from the marker.
                    google.maps.event.clearListeners($marker, 'click');
                }
               
            }
        }
    }else{
        if ($marker.infoWindow != null) {
            // Close the infoWindow of the marker.
            $marker.infoWindow.close();

            // Remove the infoWindow from the marker.
            $marker.infoWindow = null;

            // Remove the click listener from the marker.
            google.maps.event.clearListeners($marker, 'click');
        }
        
    }
    if ($icon != null && $icon != "") {
        $marker.setOptions({  icon: $icon});
    }
}

function removeMarker($markerId) {

    $.each(markers, function (i , marker) {
        if(marker != null && marker.id == $markerId){
            console.log(marker)
            marker.map = null;
            marker.setMap(null)
            markers.splice(i, 1);
            return;
        }
    });
}

function getCoordinates($id, $polygon, $url) {
    console.log($id)
    var vertices = $polygon.getPath();
    var coordinates = [];
    for (var i = 0; i < vertices.getLength(); i++) {
        var xy = vertices.getAt(i);
        coordinates.push({lat: xy.lat(), lng: xy.lng()});
    }

    var shapeId = isNaN(Number($polygon.id)) ? 0 : $polygon.id;

    var map_shap =
        {
            zoneId: Number($("#Id").val()),
            color: $polygon.fillColor,
            id: shapeId,
            coordinates: coordinates
        };
    
    $.ajax({
        url: $url,
        type: 'POST',
        data: map_shap,
        success: function (response) {
            $polygon.id = response.id;
            $('#dataTables_shapes').DataTable().ajax.reload();
        
            },
        
        error: function (xhr) {
            alert(xhr.error);
        }
    });
}

function UpdateMarkerCoordinates($marker ,$event){

    var markerId = $marker != null ? isNaN(Number($marker.id)) ? 0 : $marker.id:0;
    var map_marker =
        {
            zoneId: Number($("#Id").val()),
            id: markerId,
            coordinate: {
                lat: $event.latLng.lat(),
                lng: $event.latLng.lng(),
            }
        };

    $.ajax({
        url: "/Zones/AddMarkerOnMap",
        type: 'POST',
        data: map_marker,
        success: function (res) {
            if(res["status"] == "success"){
                if(res["id"] != null && res["id"] > 0){
                    var markersCoords = [{
                        Id: res["id"],
                        lat: $event.latLng.lat(),
                        lng: $event.latLng.lng(),
                    }];
                    console.log(markersCoords);
                    add_select_markers(markersCoords, res["id"]);
                }
                reloadIsReady();

            }
        },
        error: function (xhr) {
            alert(xhr.error);
        }
    });
}

function manageMapLocation($map) {

    _self.vars.ID = "gllpZoom_container";
    _self.vars.cssID = "#gllpZoom_container ";
    _self.vars.geocoder = new google.maps.Geocoder();
    _self.vars.elevator = new google.maps.ElevationService();
    _self.vars.map = $map;
    _self.vars.LATLNG = new google.maps.LatLng(_self.params.defLat, _self.params.defLng);
    _self.vars.MAPOPTIONS = _self.params.mapOptions;
    _self.vars.MAPOPTIONS.center = _self.vars.LATLNG;

    // for reverse geocoding
    var getLocationName = function (position) {
        var latlng = new google.maps.LatLng(position.lat(), position.lng());
        _self.vars.geocoder.geocode({'latLng': latlng}, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK && results[1]) {
                $(".gllpLocationName").val(results[1].formatted_address);
            } else {
                $(".gllpLocationName").val("");
            }
            $(_self.vars.cssID + "#gllpZoom").trigger("location_name_changed", $(_self.vars.cssID + "#gllpZoom"));

        });
    };

    // for getting the elevation value for a position
    var getElevation = function (position) {
        var latlng = new google.maps.LatLng(position.lat(), position.lng());

        var locations = [latlng];

        var positionalRequest = {'locations': locations};

        _self.vars.elevator.getElevationForLocations(positionalRequest, function (results, status) {

            if (status == google.maps.ElevationStatus.OK) {
                if (results[0]) {
                    $(".gllpElevation").val(results[0].elevation.toFixed(3));
                } else {
                    $(".gllpElevation").val("");
                }
            } else {
                $(".gllpElevation").val("");
            }


            $(_self.vars.cssID + "#gllpZoom").trigger("elevation_changed", $(_self.vars.cssID + "#gllpZoom"));

        });
    };
    var performSearch = function (string, silent) {
        if (string == "") {
            if (!silent) {
                _self.params.displayError(_self.params.strings.error_empty_field);
            }
            return;
        }
        _self.vars.geocoder.geocode({"address": string}, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    _self.vars.map.setZoom(_self.params.defZoom);


                    setPosition(results[0].geometry.location);
                } else {
                    if (!silent) {
                        _self.params.displayError(_self.params.strings.error_no_results);
                    }
                }
            }
        );
    };

    var setPosition = function (position) {
        _self.vars.map.panTo(position);

        if (_self.params.queryLocationNameWhenLatLngChanges) {
            getLocationName(position);
        }
        if (_self.params.queryElevationWhenLatLngChanges) {
            getElevation(position);
        }
    };


    $('.gllpSearchField').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            event.preventDefault();
            performSearch($(".gllpSearchField").val(), false);
        }

    });

    $('.gllpLinkField').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            event.preventDefault();
            var regex = new RegExp('@(.*),(.*),');
            var lon_lat_match = $(this).val().match(regex);
            if ($(this).val().match(regex)) {
                var lat = lon_lat_match[1];
                var lng = lon_lat_match[2];
                performSearch(lat + ',' + lng, false);
            } else {
                alert('please insert google map link');
            }
            ;
        }
    });

    $(".gllpCurrentLocation").click(function () { //user clicks button
        if ("geolocation" in navigator) { //check geolocation available 
            //try to get user current location using getCurrentPosition() method
            navigator.geolocation.getCurrentPosition(function (position) {

                performSearch(position.coords.latitude + "," + position.coords.longitude, false);
            });
        } else {
            console.log("Browser doesn't support geolocation!");
        }
    });

}

