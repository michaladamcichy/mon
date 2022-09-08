<script>
	import { onMount } from 'svelte';


	import Map from './Map.svelte';
	import SidePane from './SidePane.svelte';
	import BottomPane from './BottomPane.svelte';
	import {_stationaryStations} from '../lib/stationaryStations.js';

	import {api} from '../lib/api';
	import { test } from '../lib/test';
	import { maps } from '../lib/maps';

	export let ready;

	let map;

	const defaultRanges = [20, 30, 50];
	const defaultCounts = [10000,10000,10000];
	const defaultWeights = [1.0, 1.5, 2.5];
	const defaultStations = [];
	const defaultUnits = [];
	let instances = [
		{
			name: 'instance',
			isConnected: undefined,
			stations: [	
			],
			units: [
			],
		}
	];

	let selectedInstance;

	let ranges = defaultRanges;
	let counts = [...defaultCounts];
	let weights;
	let isConnected;
	let stations;
	let units;
	let bigTestRunning = false;
	let percentageOfStations = 0.0;
	let disableMap = false;
	let optimized = false;

	let filter = [];

	const distance = (o1, o2) => {
		return Math.pow(o1.position.lat - o2.position.lat, 2) + Math.pow(o1.position.lng - o2.position.lng, 2); 
	};


	const filterThreshold = 0;
	const updateFilter = (mapObject) => {
		if(!mapObject) {
			filter = [];
			return;
		}

		let closeOnes = [];
		(mapObject.priority !== undefined ? units : stations).forEach(_mapObject => {
			if(mapObject != _mapObject && distance(mapObject, _mapObject) < filterThreshold) closeOnes.push(mapObject);
		});

		filter = [mapObject, ...closeOnes];
	};

	const updateOptimized = (newValue) => {
		optimized = newValue;
	};

	const updateDisableMap = (newValue) => {
		disableMap = newValue;
	}

	function fisherYates( array ){
    var count = array.length,
        randomnumber,
        temp;
    while( count ){
     randomnumber = Math.random() * count-- | 0;
     temp = array[count];
     array[count] = array[randomnumber];
     array[randomnumber] = temp
    }
   }

	const loadGSM = async (percentage = percentageOfStations) => {
		selectedInstance.stations = selectedInstance.stations.filter(station => !station.isStationary);
		if(percentage == 0) {
			loadInstance(selectedInstance);
			return;
		}
		
		fisherYates(_stationaryStations);
		const stationaryStations = [];
		for(let i = 0; i < Math.floor(percentage/100 * _stationaryStations.length); i++) {
			stationaryStations.push(_stationaryStations[i]);
		}

		selectedInstance.stations = selectedInstance.stations.concat(
			stationaryStations.map(ss => ({position: ss.position, range: ss.range/*[20, 30, 50][Math.floor(Math.random() * 3)]*/, isStationary: true}))); //alert CRITICAL ALERT
		loadInstance(selectedInstance);
	};

	const updatePercentage = (newValue) => {
		percentageOfStations = newValue;
	};
	const updateBigTestRunning = (value) => {
		bigTestRunning = value;
	}
	const updateInstance = instance => {
		instance.isConnected = isConnected;
		instance.stations = stations;
		instance.units = units;

		instances = instances;
	};

	const loadInstance = instance => {
		weights = instance.weights;
		isConnected = instance.isConnected;
		stations = instance.stations;
		units = [...instance.units];

		instances = instances;
	};

	const checkIsConnected = async () => {
		return api.isConnected(ranges,
				counts,
				stations,
				units);
	};

	const connectionCheck = async () => {
			let serverNotResponding = true; //alert to chyba miała być globalna
			setTimeout(() => {if(serverNotResponding) isConnected = null;}, 5000);
			selectedInstance.isConnected = isConnected = await checkIsConnected();
			serverNotResponding = false;
		};
		
	const selectInstance = instance => {
		if(selectedInstance)
		updateInstance(selectedInstance);

		weights = instance.weights;
		isConnected = instance.isConnected;
		stations = instance.stations;
		units = instance.units;

		selectedInstance = instance;
		
		instances = instances;
		connectionCheck();
		//console.log(instances);
	};

	selectInstance(instances[0]);

	const addInstance = () => {
		//console.log('addInstance');
		//console.log(instances);
		instances.push(
			{
			name: 'instance',
			ranges: [...defaultRanges],
			weights: [...defaultWeights],
			isConnected: undefined,
			oldRanges: defaultRanges,
			stations: defaultStations,
			units: defaultUnits,
		});
		instances = instances;
	};

	const duplicateInstance = instance => {
		if(bigTestRunning) {
			alert('Cannot do that while big test is running');
			return;
		}

		instances.push(JSON.parse(JSON.stringify(instance)));
		instances = instances;
	};
	
	const priorities = [
		{priority: 4, icon: 'fa fa-exclamation'},
		{priority: 3, icon: 'fa fa-truck'},
		{priority: 2, icon: 'fa fa-star'},
		{priority: 1, icon: 'fa fa-male'},
		{priority: 0, icon: 'fa fa-wrench'},
    ];

	const updateRanges = (_ranges) => {
		ranges = [..._ranges];
	};

	const updateStationsFromRanges = (oldRanges, newRanges) => {
		let diffs = [];
		for(let i = 0; i < oldRanges.length; i++) {
			if(oldRanges[i] != newRanges[i]) {
				diffs.push(i);
			}
		}
		
		const _stations = [...stations];

		_stations.forEach(station => {
			for(let i = 0; i < diffs.length; i++) {
				if(station.range == oldRanges[diffs[i]]) {
					station.range = newRanges[diffs[i]];
					break;
				}
			}
		});

		stations = _stations;
	};

	const updateCounts = (_counts) => {
		counts = [..._counts];
	};

	onMount(() => {
		test.run();
	});

	// let serverNotResponding = true;
	$: {
		units; stations; ranges;
		updateInstance(selectedInstance);
	};

	$: {
		units; stations; ranges;
		connectionCheck();
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
            //console.log('error');
        }
    };

	const removeStation = station => {
		//console.log('removing');
		const index = stations.indexOf(station);
        if(index >= 0) {
			stations = stations.filter((item, _index) => _index != index);
            updateAllStations(stations);
        } else {
            //console.log('error');
        }
	}

	const removeInstance = instance => {
		//console.log('removing');
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
            //console.log('error');
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
            //console.log('error');
        }
	};

	const removeUnit = unit => {
		//console.log('removing');
		const index = units.indexOf(unit);
        if(index >= 0) {
            units = units.filter((item, _index) => _index != index); 
            updateAllUnits(units);
        } else {
            //console.log('error');
        }
	};

	const setMap = _map => {
		map = _map;
	};
</script>

<svelte:head>
	<script defer async
		src={`https://maps.googleapis.com/maps/api/js?key=${maps.apiKey}&callback=initMap`}
	>
	</script>
</svelte:head>

<div class="row">
	<div id="leftCol" class="col-7">
		<div id="leftTop" class="row">
			{#if ready}
			<Map
				map={map}
				setMap={setMap}
				stations={stations}
				updateStation={updateStation}
				units={units}
				updateUnit={updateUnit}
				updateFilter={updateFilter}
				/>
			{/if}
		</div>
		<div id="leftBottom" class="row">
			<BottomPane
				stations={stations}
				units={units}
				ranges={ranges}
				counts={counts}
				updateCounts={updateCounts}
				stationWeights={weights}
				updateRanges={updateRanges}
				updateStationsFromRanges={updateStationsFromRanges}
				updateStations={updateAllStations}
				updateUnits={updateAllUnits}
				checkIsConnected={checkIsConnected}
				isConnected={isConnected}
				bigTestRunning={bigTestRunning}
				updateBigTestRunning={updateBigTestRunning}
				percentageOfStations={percentageOfStations}
				updatePercentage={updatePercentage}
				loadGSM={loadGSM}
				disableMap={disableMap}
				updateDisableMap={updateDisableMap}
				optimized={optimized}
				updateOptimized={updateOptimized}
				/>
		</div>
	</div>
	<div id="rightCol" class="col-5">
			<SidePane
				map={map}
				instances={instances}
				addInstance={addInstance}
				selectInstance={selectInstance}
				loadInstance={loadInstance}
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
				ranges={ranges}
				counts={counts}
				priorities={priorities}
				bigTestRunning={bigTestRunning}
				percentageOfStations={percentageOfStations}
				filter={filter}
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
		height: 60vh;
		/* height: 98vh; */
	}
	#bottomRow {
		height: 30vh;
	}

	#leftCol {
		height: 90vh;
	}

	#rightCol {
		height: 90vh;
	}

	#leftTop {
		height: 60vh;
	}

	#leftBottom {

	}
</style>