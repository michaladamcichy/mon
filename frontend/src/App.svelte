<script>

	import Map from './Map.svelte';
	import SidePane from './SidePane.svelte';
	import BottomPane from './BottomPane.svelte';

	import { onMount } from 'svelte';

	import {maps} from '../lib/maps.js';
	import {api} from '../lib/api';

	export let ready;

	let map;

	let stationRanges = [20, 30, 50];
	let stationCounts = [1000,1000,1000];
	let isConnected = null;
	let oldRanges = [...stationRanges];
	let defaultRange = stationRanges[0];

	const updateRanges = (ranges) => {
		for(let i =0; i < ranges.length - 1; i++) {
			if(ranges[i] >= ranges[i+1]) {
				stationRanges = [...oldRanges];
				return;
			}
		}

		stationRanges = ranges;
		console.log(stations);
		console.log(oldRanges[0]);
		console.log(ranges[0]);
		updateAllStations(stations.map(station => {
			const index = oldRanges.indexOf(station.range);
			if(index >= 0) {
				station.range = ranges[index];
			} else {
				console.log('custom range');
			}
			return station;
		}));
		oldRanges = [...ranges];
	};

	let stations = [
		{position: {lat: 52.2297, lng: 21.0122}, range: stationRanges[0] },
		{position: {lat: 52.2297, lng: 21.0122}, range: stationRanges[0] },
		{position: {lat: 52.2297, lng: 21.0122}, range: stationRanges[0] },
		{position: {lat: 52.2297, lng: 21.0122}, range: stationRanges[0] },
		];

	let units = [
		{position: {lat: 51.2297, lng: 21.0122 }},
		{position: {lat: 51.2297, lng: 21.0122 }},
		{position: {lat: 51.2297, lng: 21.0122 }},
		{position: {lat: 51.2297, lng: 21.0122 }},
	];

	onMount(() => {
	});

	$: {
		(async () => {
			let serverNotResponding = true;
			setTimeout(() => {if(serverNotResponding) isConnected = null;}, 5000);
			isConnected = await api.isConnected(stationRanges, stationCounts, stations, units);
			serverNotResponding = false;
		})();
	}

	const updateAllStations = _stations => {
		stations = _stations;
	};

	const updateStation = station => {
        const index = stations.indexOf(station);
        if(index >= 0) {
            stations[index] = station;
            updateAllStations(stations);
        } else {
            console.log('error');
        }
    };

	const removeStation = station => {
		console.log('removing');
		const index = stations.indexOf(station);
        if(index >= 0) {
			stations = stations.filter((item, _index) => _index != index);
            updateAllStations(stations);
        } else {
            console.log('error');
        }
	}

	const updateAllUnits = _units => {
		units = _units;
	};

	const updateUnit = unit => {
		const index = units.indexOf(unit);
        if(index >= 0) {
            units[index] = unit;
            updateAllUnits(units);
        } else {
            console.log('error');
        }
	};

	const removeUnit = unit => {
		console.log('removing');
		const index = units.indexOf(unit);
        if(index >= 0) {
            units = units.filter((item, _index) => _index != index); 
            updateAllUnits(units);
        } else {
            console.log('error');
        }
	}

	const setMap = _map => {
		map = _map;
	}
</script>

<svelte:head>
	<script defer async
		src={`https://maps.googleapis.com/maps/api/js?key=${maps.apiKey}&callback=initMap`}
	>
	</script>
</svelte:head>

<div class="row">

	<div id="leftCol" class="col-8">
		<div id="leftTop" class="row">
			{#if ready}
			<Map map={map} setMap={setMap} stations={stations} updateStation={updateStation} units={units} updateUnit={updateUnit}/>
			{/if}
		</div>
		<div id="leftBottom" class="row">
			<BottomPane
				stations={stations}
				units={units}
				stationRanges={stationRanges}
				stationCounts={stationCounts}
				updateRanges={updateRanges}
				updateStations={updateAllStations}
				isConnected={isConnected}/>
		</div>
	</div>
	<div id="rightCol" class="col-4">
			<SidePane
				map={map}
				stations={stations}
				updateStation={updateStation}
				removeStation={removeStation}
				updateAllStations={updateAllStations}
				units={units}
				updateUnit={updateUnit}
				removeUnit={removeUnit}
				updateAllUnits={updateAllUnits}
				stationRanges={stationRanges}/>
	</div>
</div>

<style>
	div {
		border: 2px solid rgba(0,0,0,0);
		margin: 0 0 0 0 !important;
	}
	#topRow {
		 
		padding: 0;
		height: 74vh;
		/* height: 98vh; */
	}
	#bottomRow {
		height: 24vh;
	}

	#leftCol {
		height: 98vh;
	}

	#rightCol {
		height: 98vh;
	}

	#leftTop {
		height: 74vh;
	}

	#leftBottom {

	}
</style>