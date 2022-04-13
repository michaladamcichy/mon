<script>
import { onMount } from "svelte";
// import { google } from

    export let map;
    export let setMap;
    export let stations;
    export let updateStation;
    export let units;
    export let updateUnit;

    let container;
    
    let markers = [];
    let circles = [];

    $: {
        updateMarkers(map, stations, units);
        updateCircles(map, stations);
    }

    onMount(async () => {
            const _map = new google.maps.Map(container, {
                center: { lat: 53.015959, lng: 18.608620 }, //alert hardcoding
                zoom: 6,
            });

            setMap(_map);

            updateMarkers(_map, stations, units);
            updateCircles(_map, stations);
    });

    const stationMarkerIcon = {
        path: google.maps.SymbolPath.CIRCLE,
        fillColor: 'white',
        fillOpacity: 1.0,
        strokeColor: 'black',
        strokeOpacity: 1.0,
        strokeWeight: 5.0,
        scale: 15,
        fontWeight: 'bold',
    };

    const unitMarkerIcon = {
        path: google.maps.SymbolPath.CIRCLE,
        fillColor: 'white',
        fillOpacity: 1.0,
        strokeColor: 'darkgreen',
        strokeOpacity: 1.0,
        strokeWeight: 5.0,
        scale: 15,
        fontWeight: 'bold',
    };

    const generateMarkers = (map, locations, update) => {
        locations.forEach((location, index) => {
            const marker = new google.maps.Marker({
                position: location,
                map,
                label: {
                    text: '' + (index + 1),
                    fontSize: '20px',
                    fontWeight: 'bold',
                    color:'black'
                },
                draggable: true,
                icon: locations == stations ? stationMarkerIcon : locations == units ? unitMarkerIcon : null,
            });
            marker.addListener('dragend', () => {
                location.lat = marker.getPosition().lat();
                location.lng = marker.getPosition().lng();
                update(location);
            });

            markers.push(marker);
        });
    };

    const updateMarkers = (map, stations, units) => {
        markers.forEach(marker => { //alert todo nowa for loop
            marker.setMap(null);
        });
        markers = [];

        generateMarkers(map, stations, updateStation);
        generateMarkers(map, units, updateUnit);
    };

    const updateCircles = (map, stations) => {
        circles.forEach(circle => {circle.setMap(null)});
        circles = [];
        stations.forEach((unit,index) => {
            const circle = new google.maps.Circle({
                strokeColor: "#FF0000",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: "#FF0000",   
                fillOpacity: 0.1,
                map,
                center: {lat: unit.lat, lng: unit.lng},
                radius: unit.radius * 1000,
            });
            circle.bindTo('center', markers[index], 'position');
            circles.push(circle);
        });
            //console.log({lat: unit.lat, lng: unit.lng});
            
    };
</script>


<div id="map" bind:this={container}>
    <button id="resetCenterButton" class="btn primary-btn" >RESET</button>
</div>

<style>
    #map {
        height: 100%;
        width: 100%;
        margin: 0;
    }
    #resetCenterButton {
        position: absolute;
        left: 20px;
        bottom: 20px;
    }
</style>