<script>
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
    export let stationRanges;
    export let stationCounts;
    export let updateAllStations;
    export let updateAllUnits;
    export let priorities;
    export let percentageOfStations;


    let instancesHidden = false;
    let stationsHidden = false;
    let unitsHidden = false;

    const addStation = (range, isStationary = false) => {
        //console.log('add station');
        //console.log(stationRanges);
        const newStation = {position: {lat: map.getCenter().lat(), lng: map.getCenter().lng()}, range: range, isStationary};
        //console.log(newStation);
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
        //console.log('add unit');
        //console.log(newUnits);
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
    
</script>

<div id="main" class="container">
    <div class="row">
        <h4 class="col">Instances</h4>
        <button class="toggleVisibilityButton btn btn-primary" on:click={() => {instancesHidden = !instancesHidden;}}>{instancesHidden ? "v" : "^"}</button>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
    </div>
    <hr>
    {#if !instancesHidden}
        <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
            <button class="addButton btn btn-primary" on:click={() => {addInstance()}}>
                +
            </button>
            <div class="col"></div>
            <div class="col"></div>
            <div class="col"></div>
            <button class="col btn btn-danger removeAllButton" on:click={() => {removeAllInstances();}} disabled={instances.length == 0}>X</button>
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
        <button class="toggleVisibilityButton btn btn-primary" on:click={toggleStationsVisibility}>{stationsHidden ? "v" : "^"}</button>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
    </div>
    <hr>
    {#if !stationsHidden}
        <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
            <button class="addButton btn btn-primary" on:click={() => {addStation(stationRanges[0])}}>
                +
            </button>
            {#each stationRanges as range}
                <button class="col btn btn-primary" on:click={() => {addStation(range)}}>{range}</button>
            {/each}
            <div class="col">km</div>
            <button class="col btn btn-primary fa fa-star ss" on:click={() => {addStation(stationRanges[stationRanges.length-1], true)}}></button>
            <div class="col"></div>
            <div class="col"></div>
            <button class="col btn btn-danger removeAllButton" on:click={() => {removeAllStations()}} disabled={stations.length == 0}>X</button>
        </div>
        <hr>
        <hr>
        {#each stations as station, index}
            <Station index={index} station={station} update={updateStation} remove={removeStation} ranges={stationRanges}/> 
        {/each}
    {/if}
    <hr>
    <div class="row">
        <h4 class="col">Units</h4>
        <button class="toggleVisibilityButton btn btn-primary" on:click={toggleUnitsVisibility}>{unitsHidden ? 'v' : '^'}</button>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
    </div>
    <hr>
    {#if !unitsHidden}
        <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
            <button class="addButton btn btn-primary" on:click={() => {addUnit();}}>
                +
            </button>
            {#each priorities as priority}
                <button
                    class="col btn selectButton btn-primary"
                    on:click={() => {addUnit(priority.priority)}}>
                    <i class={priority.icon}></i>
                </button>
            {/each}
            <div class="col"></div>
            <div class="col"></div>
            <button class="col btn btn-danger removeAllButton" on:click={() => {removeAllUnits()}}
                disabled={units.length == 0}>X</button>
        </div>
        <hr>
        <hr>
        {#each units as unit, index}
            <Unit index={index} unit={unit} update={updateUnit} remove={removeUnit}
                ranges={stationRanges} priorities={priorities} addUnit={addUnit}/>
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
    
</style>