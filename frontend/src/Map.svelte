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

    let selectedToCopyPosition = null;

    $: {
        updateMarkers(map, stations, units);
        updateCircles(map, stations);
    }

    onMount(async () => {
            const _map = new google.maps.Map(container, {
                center: { lat: 53.015959, lng: 18.608620 }, //alert hardcoding
                zoom: 6,
                mapId: '4b9343d90f363d08',
            });

            setMap(_map);

            updateMarkers(_map, stations, units);
            updateCircles(_map, stations);
    });

    const stationMarkerIcon = station => {
        return {
            path: google.maps.SymbolPath.CIRCLE,
            fillColor: station.isStationary ? 'yellow' : 'white',
            fillOpacity: 1.0,
            strokeColor: station.isStationary ? 'white' : 'black',
            strokeOpacity: 1.0,
            strokeWeight: 5.0,
            scale: 13,
            fontWeight: 'bold',
        };
    };

    const unitMarkerIcon = unit => {
        switch(unit.priority) {
            case 0: 
            return "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAABmJLR0QA/wD/AP+gvaeTAAAEZ0lEQVRIicXWaUxUVxQA4HPfm/3Nxr6IdSQjIsooJQpJW5ZaYFoqXVJa1JASNDq1hAISkhaTkjbaFiO2hCo0QkxLQ2MpTZUudkBGBCQKsUFQrFhZCzIMwzrLm/fm9oeUUEKV5RHvz5dzzpdz3r25F+AJLbQaRXfEZ8RLJMJPWMz6OBzOoeafCjQAgOfG8DgV8/KIyA5rrUxERgSpfYi7febJ4bGxovkoAADBpRt1y1LiIedHxjyrIfpNtpHB0bH0xsqCkoViORu1JjaZUsi9jdEhKnH3CG38s6cvp7mq4Oz/mJizjhlW+rQrJRAFBgaAaWzSbpkyfj8/Jvz1rJTtCVkNAByO2glYQRKAlC5KiAwLduPx3BLno/5+PvlSSqTkFJY6MWmjWYZlWAgLCRSLRfyPtsVnvq2JTab+RVPejPNgWOffAAAkF+jGmDRffx6uZWlGIHeTEQFqFUSEbVG4yqVxoxOOfU/5emnTU1/1rKxuGL19pzdz+P71v1a8uVRRKaJNfFHnhxJmnZAS4Swbn8l4Zzd/jbfHbAzGGPT1rdbqS9d/bqrKTwTg4BxvEQr1+4XsuiCZAMQUHxUJMS+n8Ntpz/V+NpX/WonVTtOtbXenp+228iYN9T5UPcxbUcfxcbqvXuTjfXtkiJDJxQAA0GV3QtYQPTRkh4NW5MQEIsYmLKKW/qsnrXNzl93xy7G6PA3pTE2igJDKJAAAcHmapT9+QHeZEKFtrzvV96j8ZcEvxerSA0mcm0FhUqagACGAM2bHeIWZbeh12N7oNpy1Pa7GkuGdsbq960l8/DDl5CmUFBAIQb7RMVYzjcvq9acPL7bOkmBtjO6gioTCD6ROgYtSAphAkPuAHm21wom6i18eW0qtRW+uXdpDmWpgP8uRYb6rQgwsiSBrkDZ1WCDvSs2poqWgi4Z3aXXF4aRz/wEKkQqFGJwEgrQB2thuY9Oaa4rPLRUFeMyoQ0MP8L3d0a9xPBy9WzpzZBCCI0O0qcuOc5aLPhIO3Pmu2xoeezVZwKhfkPGRVCYCAIBjww5Tq5U9atAXL3TlrQwOiU7d6sdj67KFrMs2hQDEEiEAAJSZGbNhii2p0xefXAn6H3jjM6kydy+Pozw+EWMzj6s281jBJhcRiEUPQ36ZYmwV48zvBv3p3JWis/DmqENSpZvsWkpirHprkIqHAMHNjnsO3XfVE6WeWNnrwPgLo6PNoDDt5QIFmLkWN4RGH09+7XltbeMN84+/NQ729A8z4duD5F5+vuT5ltvWr83M4CByRBjPl1u4ggkAAJJA2ktNf4x29w6/d/ncpxvqW26+VVl9ZVgTrObXWAANMI5XOi6WjnKFzsJWm328t38k/dqFExUAAJ01xYbO+wPjGGOgFPKe9tozbVyiADP/uOXC5zvmfgxLyE7ydlMob3TcY1mW1XONAizw5gpLyE5a6+de+Fx4sMs3P9TeMZsmj6wGPP/NhXwCwsvsNO1sv9VdPjE1uKe9tnR6NeAntv4BFOS/I2FCxYEAAAAASUVORK5CYII="; 
                        break;
            case 1:
            return "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAABmJLR0QA/wD/AP+gvaeTAAAEVUlEQVRIieWWTWxUVRTHf/e9mc5M2yltmWk71GBqoYhQJBRi+NAqfpAYDIkKLoiCrHBhWMDChTsXaCRGE4y6AlQIBF34tdBgVKIGIxIgCFqKKAZaSjtOB2bee/PevcfFFBpKyxu1rjjJTe7LOff/u/ecm/Mu3Gqm/sWaucBKoGPkuwf4BPj5/wJPBd6pytQ/3vhgp4q3pQFwfxsg++UJKfXnPgA2AtnJBDej1PeZZ7vvyDx9HypqX+cUX9O38xv63j14BpHFwKUwQTssYMQ+bt340ILMum6Ubd3gVLZFsqsNZVuNl4+cnQ+8FyZ4o8qNtrx6ZsvyzNploYGZZ5aRaG9+GOieDPCa1GNdiKqgKsoitXIBwJrJAHcm78xUEFa25JxpAPMmA3xbrClZMTiergOYHhYXCfGvSLQ1TVdTp9Bpsjyq+qmzFMqKoFT5XopoxATkjfCZtHAi1UCirWm6c3ZgBfD5RMJhhfux4411C2enouxqOU68Ko4diWNFYiirvGcxASbw0IFLwXXZcLGTU0MBPZt2HQYWTSQcluo6c/o8Lxa/I6pMSCjEbcOWoYPo0+cBam8WGwZ+onf7AefCxTyIuZbWcYdoEEN/Ns+Z7QeKwJP/BXwC2PzqvixG+4j2Mca/lloduJjAw5gRn/Z5/cM8wGZCenclt3r/yd894zheGRJ4aO1hAhcTuCPz8ig6Hif/8DSwP0y0kpZZdErSobWe1313HBEBoxEJEOMjQQmjS4j2eGnnRb4+6u4G9kwGGOCrQ6fcVYhJLZkTAdFl6NX0a4+t719i277hk8BqwA0TrCTVAFNE+GXr7r/oHY7iuKVr6XXcEr35Kl7ekwP4FZhSiWDYiVuBT62a2Gv1T3XNblm9mIH2mczKXaDkaK44cMmP8WbjUobab8eaVju7dGZwkwT6AeAL4PJEwjdrIK9gW1sa1y601IwGRBtal3YRjcdpEIdV2eMAfNQ4jywxvOECfT8dQykLczpLbu8RjTbbgBf+CXi9nanbUbdhESpqk2xtoba1GSsSwY7Y2FXR64J9x8N3XIzvU+gfpDgwiPia/I7D6L78emBXpeCe9PP3z6xZ0EayJQ2WVb7NIqAgVls9ulSgmMsjgUZEykNrioNZnON/kn372x5g1ljAeJcrE6mvbm9+ZCHJTBMCGG0QbTDaYHxN4Pmjp3VdTMnHaI3RGtEGEUg0NlC/5C7susQMoGUsZLy/09xER8bCtjDakDt7DuP7IFDdnCZak8C7UsCybRDBzRcQbfALRZzBLCAo26a6OY0oiLWlreKxc51Afxg4oaJ2efdGKPSPvtvsWAw7HgMjFIdyiAhmJMWl/BW84fyoSCoFClQsApAYCxmvxk1WPNqbXndvMlKXwIuWyvUF7MDCDhRXyw0jEwEdFUxURk9UVOi8S3bvocviBTOAgTAwwHzgOeAeyu/pq6YpN//CmPgaYA7X94Uh4AfgLeDoBJxbyP4GG9UbhswCS9UAAAAASUVORK5CYII=";
                break;
            case 2:
            return "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABkAAAAZCAYAAADE6YVjAAAABmJLR0QA/wD/AP+gvaeTAAAC40lEQVRIieWVS0gUcRzHPzM7+9C2TS136QVFmKWUSVsWIVkKPbCDZdApokuHrMBrEVR0EDoE0aXaQ9Hj1CmjQxb0AiFs7UmG9KCnlYaptM/5ddj/rOPuWksJHfrBj/nPzG++n99j5j/wP1oV0AmMAHeAmRMN8ALvALH5Y2DSREL2AKLryOEWr2haGtQyUQAH0AvIpjq3yOOANK52W5Bedf+vrUkJys1Qscgjv9w4XWRvW9OfiPqACqAB2A6EAVk83xCz2y/S7RcJl0r1AsOChFVcg3rO9yvxDcADxg437eeO+kS6S0XCpSIPpsnZQ96cccq7gPWWsKaOC4AnZPTXcEBgqk75HINrJ6fgcoiSEWIxYcPeIXreJOkbMEkks5IeAYLAcwvSApwo9umEjviYHXAws1QnUKKhaynRlGevRQQzKfQNCO8/J3nbZ7KrLcLXQQE4CBzRFaQfYHDY5NMXk2CFwfRpGromNtFsF3XUNZheAsFynU/9wsCQWNV8xNaeV8BaEWZdvR0lEhXqlznRfpF9rutt56O0nogiKcYzYB8QtyBxoB1YA8y4F47z+kOSxlq3rRrGZG+HJJOw+1iEtotxq4IuYB3wzV6JNahLaljzHr5I8rAnweY6F4aeEs0FiMZg64EfXOxIWDrXgY3AQObbZTcXcBlodBoweLMIj4uc7UGESBSKNg4TSxXRDmwBYnZBnWyLWQOrKnMowPjD9ziFRXPTMh8zAeNBAGoAaiqN3K+uKcpT65qFaZnlucRyQSaR2h5YWq6nBc2EELoSY8mOEULtcUxztJrg/HTXK8lz+69F5f70glfMu5Ol81ShrKh0jNk6qst0uXXcLWaHR56ccdnv1eYDqQdE05D7oULZVm/Y/x8CvLXWmoZsq9Ol66RT9NGYrflAZgAJe9bKXwLNKqZZnWfGmEB5PhCAnaQ+UAEGgf2AJyPGo65/V3EJoDVfgGV+YBVQ8Ju4AmCliv939hM7a5u4Et0c8AAAAABJRU5ErkJggg==";
                break;
            case 3:
            return "data:image/png;base64,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";
            break;
            case 4:
            return "data:image/png;base64,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";
            break;
        }
        return {
            path: google.maps.SymbolPath.CIRCLE,
            fillColor: 'white',
            fillOpacity: 1.0,
            strokeColor: 'darkgreen',
            strokeOpacity: 1.0,
            strokeWeight: 5.0,
            scale: 15,
            fontWeight: 'bold',
        };
    };

    const getUnitIcon = (unit) => {
        //const icons = {4: google.maps.SymbolPath.,}
        let shape;

        //return {...unitMarkerIcon, path: goo}
    };


    const generateMarkers = (map, mapObjects, update) => {
        //console.log('HERE');
        //console.log(mapObjects);
        mapObjects.forEach((mapObject, index) => {
            const marker = new google.maps.Marker({
                position: mapObject.position,
                map,
                label: {
                    text: mapObjects == stations ? '' + (index + 1) : ' ', //alert
                    fontSize: '20px',
                    fontWeight: 'bold',
                    color:'black'
                },
                draggable: true,
                icon: mapObjects == stations ? stationMarkerIcon(mapObject) : mapObjects == units ? unitMarkerIcon(mapObject) : null,
            });
            marker.addListener('dragend', e => {
                mapObject.position.lat = marker.getPosition().lat();
                mapObject.position.lng = marker.getPosition().lng();
                update(mapObject);
            });

            marker.addListener('click', e => {
                //console.log('start');
                if(!e.domEvent.shiftKey) {
                    //console.log('!shift');
                    selectedToCopyPosition = null;
                    return;
                }

                if(!selectedToCopyPosition) {
                    //console.log('!selectedToCopy');
                    selectedToCopyPosition = marker;
                    return;
                }

                //console.log('else');
                mapObject.position.lat = selectedToCopyPosition.getPosition().lat();
                mapObject.position.lng = selectedToCopyPosition.getPosition().lng();
                update(mapObject);
                selectedToCopyPosition = null;
            });

            markers.push(marker);
        });
    };

    const updateMarkers = (map, stations, units) => {
        //return;
        markers.forEach(marker => { //alert todo nowa for loop
            marker.setMap(null);
        });
        markers = [];

        generateMarkers(map, stations, updateStation);
        generateMarkers(map, units, updateUnit);
    };

    const updateCircles = (map, stations) => {
        //return;
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
                center: unit.position,
                radius: unit.range * 1000,
            });
            circle.bindTo('center', markers[index], 'position');
            circles.push(circle);
        });
            ////console.log({lat: unit.lat, lng: unit.lng});
            
    };
</script>


<div id="map" bind:this={container} >
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