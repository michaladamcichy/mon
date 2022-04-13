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
    export let defaultRadius;
    export let updateAllStations;
    export let updateAllUnits;

    let stationsHidden = false;
    let unitsHidden = false;

    const addStation = () => {
        const location = map.getCenter();
        const newStation = [...stations, {lat: map.getCenter().lat(), lng: map.getCenter().lng(), radius:   defaultRadius}];
        updateAllStations(newStation);
    };

    const toggleStationsVisibility = () => {
        stationsHidden = !stationsHidden;
    };

    const addUnit = () => {
        const location = map.getCenter();
        const newUnit = [...units, {lat: map.getCenter().lat(), lng: map.getCenter().lng()}];
        updateAllUnits(newUnit);
    };

    const toggleUnitsVisibility = () => {
        unitsHidden = !unitsHidden;
    };
    
</script>

<div id="main" class="container">
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
        {#each stations as station, index}
            <Station index={index} station={station} update={updateStation} remove={removeStation} /> 
        {/each}
        <div class="row">
            <button class="addButton btn btn-primary" on:click={() => {addStation();}}>
                +
            </button>
        </div>
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
        {#each units as unit, index}
            <Unit index={index} unit={unit} update={updateUnit} remove={removeUnit}/>
        {/each}
        <div class="row">
            <button class="addButton btn btn-primary" on:click={() => {addUnit();}}>
                +
            </button>
        </div>
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