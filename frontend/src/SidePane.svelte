<script>
    import Station from './Station.svelte';
    import Unit from './Unit.svelte';

    export let map;
    export let stations;
    export let updateStation;
    export let removeStation;
    export let units;
    export let updateUnit;
    export let removeUnit;
    export let stationRanges;
    export let updateAllStations;
    export let updateAllUnits;

    let stationsHidden = false;
    let unitsHidden = false;

    const addStation = () => {
        const newStation = [...stations, {position: {lat: map.getCenter().lat(), lng: map.getCenter().lng()}, range: stationRanges[0]}];
        updateAllStations(newStation);
    };

    const toggleStationsVisibility = () => {
        stationsHidden = !stationsHidden;
    };

    const addUnit = () => {
        const newUnit = [...units,
            {
                position: {lat: map.getCenter().lat(), lng: map.getCenter().lng()}, 
                priority: 1,
            }
        ];
        updateAllUnits(newUnit);
    };

    const toggleUnitsVisibility = () => {
        unitsHidden = !unitsHidden;
    };

    const removeAllStations = () => {
        updateAllStations([]);
    };

    const removeAllUnits = () => {
        updateAllUnits([]);
    };
    
</script>

<div id="main" class="container">
    <div class="row">
        <h4 class="col">Stations</h4>
        <button class="toggleVisibilityButton btn btn-primary" on:click={toggleStationsVisibility}>{stationsHidden ? "v" : "^"}</button>
        {#if stations.length > 0}
            <div class="col"></div>
            <button class="col btn btn-danger" on:click={() => {removeAllStations()}}>X</button>
        {/if}
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
    </div>
    <hr>
    {#if !stationsHidden}
        <div class="row">
            <button class="addButton btn btn-primary" on:click={() => {addStation();}}>
                +
            </button>
        </div>
        {#each stations as station, index}
            <Station index={index} station={station} update={updateStation} remove={removeStation} ranges={stationRanges}/> 
        {/each}
    {/if}
    <hr>
    <div class="row">
        <h4 class="col">Units</h4>
        <button class="toggleVisibilityButton btn btn-primary" on:click={toggleUnitsVisibility}>{unitsHidden ? 'v' : '^'}</button>
        {#if units.length > 0}
            <div class="col"></div>
            <button class="col btn btn-danger" on:click={() => {removeAllUnits()}}>X</button>
        {/if}
            <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
        <div class="col"></div>
    </div>
    <hr>
    {#if !unitsHidden}
        <div class="row">
            <button class="addButton btn btn-primary" on:click={() => {addUnit();}}>
                +
            </button>
        </div>
        {#each units as unit, index}
            <Unit index={index} unit={unit} update={updateUnit} remove={removeUnit}/>
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
        width: 50px;
        height: 40px;
        font-size: 20px;
    }
    #main {
        height: 98vh;
        overflow-y: auto;
        overflow-x: hidden;
    }
</style>