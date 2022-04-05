<script>

	import Map from './Map.svelte';
	import SidePane from './SidePane.svelte';
	import BottomPane from './BottomPane.svelte';

	import {maps} from '../lib/maps.js';
	import { onMount } from 'svelte';

	export let ready;

	let map;

	let defaultRadius = 50000;

	let stations = [
		{lat: 52.2297, lng: 21.0122, radius: defaultRadius },
		{lat: 52.2297, lng: 21.0122, radius: defaultRadius },
		{lat: 52.2297, lng: 21.0122, radius: defaultRadius },
		{lat: 52.2297, lng: 21.0122, radius: defaultRadius },
		];

	let units = [
		{lat: 51.2297, lng: 21.0122 },
		{lat: 51.2297, lng: 21.0122 },
		{lat: 51.2297, lng: 21.0122 },
		{lat: 51.2297, lng: 21.0122 },
	];

	onMount(() => {
	});

	const updateAllStations = _stations => {
		stations = _stations;
		console.log(stations);
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
            delete stations[index];
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
		const index = stations.indexOf(unit);
        if(index >= 0) {
            delete units[index];
            updateAllStations(stations);
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

<div id="topRow"class="row">
	<div class="col-8">
		{#if ready}
		<Map map={map} setMap={setMap} stations={stations} updateStation={updateStation} units={units} updateUnit={updateUnit}/>
		{/if}
	</div>
	<div class="col-4">
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
			defaultRadius={defaultRadius}/>
	</div>
</div>
<!-- <div id="bottomRow" class="row">
	<BottomPane />
</div> -->

<style>
	div {
		border: 2px solid rgba(0,0,0,0);
		margin: 0 0 0 0 !important;
	}
	#topRow {
		 
		padding: 0;
		/* height: 74vh; */
		height: 98vh;
	}
	#bottomRow {
		height: 24vh;
	}
</style>