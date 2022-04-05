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

    const addStation = () => {
        const location = map.getCenter();
        const newStation = [...stations, {lat: map.getCenter().lat(), lng: map.getCenter().lng(), radius:   defaultRadius}];
        updateAllStations(newStation);
    };

    const addUnit = () => {
        const location = map.getCenter();
        const newUnit = [...units, {lat: map.getCenter().lat(), lng: map.getCenter().lng()}];
        updateAllUnits(newUnit);
    };
    
</script>

<div id="main">
    <h4>Stations</h4>
    {#each stations as station, index}
        <Station index={index} station={station} update={updateStation} remove={removeStation} /> 
    {/each}
    <div class="row">
        <button class="btn btn-primary" on:click={() => {addStation();}}>
            +
        </button>
    </div>
    <hr>
    <h4>Units</h4>
    {#each units as unit, index}
        <Unit index={index} unit={unit} update={updateUnit} remove={removeUnit}/>
    {/each}
    <div class="row">
        <button class="btn btn-primary" on:click={() => {addUnit();}}>
            +
        </button>
    </div>
</div>


<style>
    button {
        margin-left: 20px;
        width: 50px;
        height: 50px;
        font-size: 20px;
        font-weight: bold;
        /* border-radius: 30px; */
    }
    #main {
        height: 98vh;
        overflow-y: auto;
        overflow-x: hidden;
    }
</style>