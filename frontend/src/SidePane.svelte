<svelte:options accessors />
<script>
    import {afterUpdate} from 'svelte';
import {resource} from '../lib/resource.js';
import Instance from './Instance.svelte';

    import Station from './Station.svelte';
    import Unit from './Unit.svelte';
    
    export let map;
    export let instances;
    export let addInstance;
    export let selectInstance;
    export let removeInstance;
    export let duplicateInstance;
    export let removeAllInstances;
    export let selectedInstance;
    export let loadInstance;
    export let stations;
    export let updateStation;
    export let removeStation;
    export let units;
    export let updateUnit;
    export let removeUnit;
    export let ranges;
    export let stationCounts;
    export let updateAllStations;
    export let updateAllUnits;
    export let priorities;
    export let percentageOfStations;


    let instancesHidden = false;
    let stationsHidden = false;
    let unitsHidden = false;

    const priorityToIcon = (priority) => {
        if(priority == 4) return resource.CHIEF_ICON;
        if(priority == 3) return resource.LADDER_ICON;
        if(priority == 2) return resource.STAR_ICON;
        if(priority == 1) return resource.SOLDIER_ICON;
        if(priority == 0) return resource.SPANNER_ICON;
    };

    const __reverse = (arr) => {
        let _arr = [...arr];
        return _arr.reverse();
    };

    const addStation = (range, isStationary = false) => {
        const newStation = {position: {lat: map.getCenter().lat(), lng: map.getCenter().lng()}, range: range, isStationary};
        updateAllStations([...stations, newStation]);
    };

    const toggleStationsVisibility = () => {
        stationsHidden = !stationsHidden;
    };

    const addUnit = (priority, master) => {
        let newUnits = [...units,
            {
                position: {lat: map.getCenter().lat(), lng: map.getCenter().lng()}, 
                priority: priority != undefined ? priority : 1, //alert
                master: priority > 0 ? master : undefined,
                name: 'Unit',
            }
        ];
        updateAllUnits(newUnits);
    };

    const toggleUnitsVisibility = () => {
        unitsHidden = !unitsHidden;
    };

    const removeAllStations = () => {
        const stationary= stations.filter(station => station.isStationary);
        const _stations = stationary.length != stations.length ? stationary : [];
        updateAllStations(_stations);
    };

    const removeAllUnits = () => {
        updateAllUnits([]);
    };

    let localRanges = ranges;

    afterUpdate(() => {
        localRanges = ranges;
    });
    
</script>

<div id="main" class="container">
    <div class="row">
        <h4 class="col">Instances</h4>
        <button class="toggleVisibilityButton btn btn-light" on:click={() => {instancesHidden = !instancesHidden;}}>{instancesHidden ? "v" : "^"}</button>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
    </div>
    <hr>
    {#if !instancesHidden}
        <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
            <button class="addButton btn btn-light" on:click={() => {addInstance()}}>
                +
            </button>
            <div class="col"></div>
            <div class="col"></div>
            <div class="col"></div>
            <button class="col btn btn-danger removeAllButton big"  on:click={() => {removeAllInstances();}} disabled={instances.length == 0}>
                <i class={'fa fa-trash'}></i></button>
        </div>
        <hr>
        <hr>
        {#each instances as instance, index}
            <Instance index={index} instance={instance} select={selectInstance} remove={removeInstance} selected={selectedInstance} duplicate={duplicateInstance}
                load={loadInstance} percentageOfStations={percentageOfStations}/>
        {/each}
    {/if}
    <div class="row">
        <h4 class="col">Stations</h4>
        <button class="toggleVisibilityButton btn btn-light" on:click={toggleStationsVisibility}>{stationsHidden ? "v" : "^"}</button>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
    </div>
    <hr>
    {#if !stationsHidden}
        <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
            <button class="addButton btn btn-light" on:click={() => {addStation(localRanges[0])}}>
                +
            </button>
            {#each localRanges as range}
                <button class="col btn btn-light bold" on:click={() => {addStation(range)}}>{range}</button>
            {/each}
            <div class="col-1"><p>km</p></div>
            <!-- <button class="col btn btn-light fa fa-star ss" on:click={() => {addStation(100.0, true)}}></button> -->
            <div class="col"></div>
            <div class="col"></div>
            <button class="col btn btn-danger removeAllButton" on:click={() => {removeAllStations()}} disabled={stations.length == 0}>
                <i class={'fa fa-trash'}></i></button>
        </div>
        <hr>
        <hr>
        {#each stations as station, index}
            <Station index={index} station={station} update={updateStation} remove={removeStation} ranges={localRanges}/> 
        {/each}
    {/if}
    <hr>
    <div class="row">
        <h4 class="col">Units</h4>
        <button class="toggleVisibilityButton btn btn-light" on:click={toggleUnitsVisibility}>{unitsHidden ? 'v' : '^'}</button>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
    </div>
    <hr>
    {#if !unitsHidden}
        <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
            <button class="addButton btn btn-light" on:click={() => {addUnit();}}>
                +
            </button>
            {#each __reverse(priorities) as priority}
                <button
                    class="col btn selectButton btn-light bold"
                    on:click={() => {addUnit(priority.priority)}}>
                    <!-- <i class={priority.icon}></i> -->
                    {priority.priority}
                    <!-- <img src={priorityToIcon(priority.priority)} alt=""> -->
                </button>
            {/each}
            <p class="col"><span>(priority)</span></p>
            <div class="col"></div>
            <div class="col"></div>
            <button class="col btn btn-danger removeAllButton" on:click={() => {removeAllUnits()}}
                disabled={units.length == 0}><i class={'fa fa-trash'}></i></button>
        </div>
        <div class="row">
            <!-- <div class="col"></div> -->
            
        </div>
        <hr>
        <hr>
        {#each units as unit, index}
            <Unit index={index} unit={unit} update={updateUnit} remove={removeUnit}
                ranges={localRanges} priorities={priorities} addUnit={addUnit}/>
        {/each}
    {/if}
    <hr>
</div>


<style>
    .addButton {
        margin-left: 20px;
        width: 50px;
        height: 50px;
        font-size: 20px;
        font-weight: bold;
        /* border-radius: 30px; */
    }
    .toggleVisibilityButton {
        width: 40px;
        height: 20px;
        font-size: 10px;
    }
    #main {
        height: 98vh;
        overflow-y: auto;
        overflow-x: hidden;
    }
    .removeAllButton {
        max-width: 40px !important;
        min-width: 40px !important;
    }

    #x:nth-child(even) {
        background-color: lightblue;
    }

    .ss {
       font-size: 20px;
    }
    
    .big {
        font-size: 20px;
    }

    .bold {
        font-weight: bold;
    }
</style>