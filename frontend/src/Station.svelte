<script>
import { onMount, update_await_block_branch } from "svelte/internal";


export let index;
export let station;
export let update;
export let remove;
export let ranges;

let lat;
let lng;

const latlngStep = 0.1;

// onMount(() => {
//     lat = station.lat;
//     lng = station.lng;
// });

</script>

<div id="main" class="container">
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
        <label class="col">{`${index + 1}.`}</label>
        <label class="col col-form-label">lat:</label>
        <input type="number" class="col" bind:value={station.lat} on:change={() => {update(station);}} step={latlngStep}/>
        <label class="col col-form-label">lng:</label>
        <input type="number" class="col" bind:value={station.lng} on:change={() => {update(station);}} step={latlngStep}/>
        <button id="removeButton" class="btn btn-danger" on:click={() => {remove(station);}}>X</button>
    </div>
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
        <label class="col">{''}</label>
        <label class="col">{'range:'}</label>
        {#each ranges as range}
            <button class={`col btn ${station.radius == range ? 'btn-success' : 'btn-primary'}`} on:click={() => {station.radius = range; update(station);}}>{range}</button>
        {/each}
        <label class="col">{'km'}</label>
        <label class="col">{''}</label>
        <label class="col">{''}</label>
        <label class="col">{''}</label>
    </div>
</div>

<style>
    #main {
        margin-top: 5px;
        margin-bottom: 5px;
    }
    input {
        min-width: 100px;
        margin-right: 20px;
    }
    #removeButton {
        width: 30px;
        height: 30px;
        font-size: 10px;
        font-weight: bold;
    }
    .row {
        margin-bottom: 5px;
    }
</style>