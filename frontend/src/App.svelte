<script>

	import Map from './Map.svelte';
	import SidePane from './SidePane.svelte';
	import BottomPane from './BottomPane.svelte';

	import { onMount } from 'svelte';

	import {maps} from '../lib/maps.js';
	import {api} from '../lib/api';

	export let ready;

	let map;

	const defaultRanges = [20, 30, 50];
	const defaultCounts = [0,0,0];
	const defaultWeights = [1.0, 1.5, 2.5];
	const defaultStations = [];
	const defaultUnits = [];
	let instances = [
		{
			ranges: defaultRanges,
			counts: [0,0,0],
			weights: [1.0, 1.5, 2.5],
			isConnected: undefined,
			oldRanges: defaultRanges,
			stations: [
				{position: {lat: 52.2297, lng: 21.0122}, range: defaultRanges[0] },
				{position: {lat: 52.2297, lng: 21.0122}, range: defaultRanges[0] },
				{position: {lat: 52.2297, lng: 21.0122}, range: defaultRanges[0] },
				{position: {lat: 52.2297, lng: 21.0122}, range: defaultRanges[0] },
			],
			units: [
				{position: {lat: 51.2297, lng: 21.0122 }, priority: 1, master: undefined},
				{position: {lat: 51.2297, lng: 21.0122 }, priority: 1, master: undefined},
				{position: {lat: 51.2297, lng: 21.0122 }, priority: 1, master: undefined},
				{position: {lat: 51.2297, lng: 21.0122 }, priority: 1, master: undefined},
			],
		}
	];

	let selectedInstance;

	let ranges;
	let counts;
	let weights;
	let isConnected;
	let oldRanges;
	let stations;
	let units;


	const updateInstance = instance => {
		instance.ranges = ranges;
		instance.counts = counts;
		instance.weights = weights;
		instance.isConnected = isConnected;
		instance.oldRanges = oldRanges; //alert czy to nie może być lokalna zmienna?
		instance.stations = stations;
		instance.units = units;

		instances = instances;
	};

	const connectionCheck = async () => {
			let serverNotResponding = true; //alert to chyba miała być globalna
			setTimeout(() => {if(serverNotResponding) isConnected = null;}, 5000);
			selectedInstance.isConnected = isConnected = await api.isConnected(ranges,
				counts,
				stations,
				units);
			serverNotResponding = false;
		};
		
	const selectInstance = instance => {
		if(selectedInstance)
		updateInstance(selectedInstance);

		ranges = instance.ranges;
		counts = instance.counts;
		weights = instance.weights;
		isConnected = instance.isConnected;
		oldRanges = instance.oldRanges; //alert czy to nie może być lokalna zmienna?
		stations = instance.stations;
		units = instance.units;

		selectedInstance = instance;
		
		instances = instances;
		connectionCheck();
		console.log(instances);
	};

	selectInstance(instances[0]);

	const addInstance = () => {
		console.log('addInstance');
		console.log(instances);
		instances.push(
			{
			ranges: [...defaultRanges],
			counts: defaultCounts,
			weights: [...defaultWeights],
			isConnected: undefined,
			oldRanges: defaultRanges,
			stations: defaultStations,
			units: defaultUnits,
		});
		instances = instances;
	};

	const duplicateInstance = instance => {
		instances.push(JSON.parse(JSON.stringify(instance)));
		instances = instances;
	};
	
	const priorities = [
		{priority: 4, icon: 'fa fa-exclamation'},
		{priority: 3, icon: 'fa fa-truck'},
		{priority: 2, icon: 'fa fa-star'},
		{priority: 1, icon: 'fa fa-male'},
		{priority: 0, icon: 'fa fa-bitbucket'},
    ];

	const updateRanges = (ranges) => {
		for(let i =0; i < ranges.length - 1; i++) {
			if(ranges[i] >= ranges[i+1]) {
				ranges = [...oldRanges];
				return;
			}
		}
		ranges = ranges;
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

	const updateCounts = () => {
		let runningCounts = [0,0,0];
		units.forEach(unit => {
			if(unit.priority === 0)
			{
				unit.counts.forEach((count, index) => {
					runningCounts[index] += count;
				});
			}
		});

		counts = runningCounts;
	};

	onMount(() => {
	});

	// let serverNotResponding = true;
	$: {
		units; stations;
		connectionCheck();
	};

	$: {
		units;
		updateCounts();
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

	const removeInstance = instance => {
		console.log('removing');
		const index = instances.indexOf(instance);
        if(index >= 0) {
			instances = instances.filter((item, _index) => _index != index);
            instances = instances;

			if(instances.length == 0)
			{
				addInstance();
				selectInstance(instances[0]);
			}

			if(instance == selectedInstance)
			{
				selectInstance(instances[0]);
			}
        } else {
            console.log('error');
        }
	}

	const removeAllInstances = () => {
		instances = [];
		addInstance();
		selectInstance(instances[0]);
	};

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
			<Map
				map={map}
				setMap={setMap}
				stations={stations}
				updateStation={updateStation}
				units={units}
				updateUnit={updateUnit}/>
			{/if}
		</div>
		<div id="leftBottom" class="row">
			<BottomPane
				stations={stations}
				units={units}
				stationRanges={ranges}
				stationCounts={counts}
				stationWeights={weights}
				updateRanges={updateRanges}
				updateStations={updateAllStations}
				isConnected={isConnected}/>
		</div>
	</div>
	<div id="rightCol" class="col-4">
			<SidePane
				map={map}
				instances={instances}
				addInstance={addInstance}
				selectInstance={selectInstance}
				removeInstance={removeInstance}
				duplicateInstance={duplicateInstance}
				removeAllInstances={removeAllInstances}
				selectedInstance={selectedInstance}
				stations={stations}
				updateStation={updateStation}
				removeStation={removeStation}
				updateAllStations={updateAllStations}
				units={units}
				updateUnit={updateUnit}
				removeUnit={removeUnit}
				updateAllUnits={updateAllUnits}
				stationRanges={ranges}
				stationCounts={counts}
				priorities={priorities}
				/>
	</div>
</div>

<style>
	div {
		border: 2px solid rgba(0,0,0,0);
		margin: 0 0 0 0 !important;
	}
	#topRow {
		 
		padding: 0;
		height: 70vh;
		/* height: 98vh; */
	}
	#bottomRow {
		height: 20vh;
	}

	#leftCol {
		height: 90vh;
	}

	#rightCol {
		height: 90vh;
	}

	#leftTop {
		height: 70vh;
	}

	#leftBottom {

	}
</style>