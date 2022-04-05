<script>
import { update_await_block_branch } from "svelte/internal";


export let index;
export let unit;
export let update;
export let remove;

const latlngStep = 0.1;
</script>

<div class="container">
    <div id="controlsContainer" class="form-group row d-flex justify-content-center align-items-center">
        <label class="col">{`${index + 1}.`}</label>
        <label class="col col-form-label">lat:</label>
        <input type="number" class="col" bind:value={unit.lat} on:change={() => {unit = unit; update(unit);}} step={latlngStep}/>
        <label class="col col-form-label">lng:</label>
        <input type="number" class="col" bind:value={unit.lng} on:change={() => {unit = unit; update(unit);}} step={latlngStep}/>
        <button class="btn btn-danger" onClick={() => {remove(unit);}}>X</button>
        <!-- <button class="col btn btn-primary" on:click={() => {}}>Pinpoint</button> -->
    </div>
    <!-- <div class="row">
        <input type="text">
    </div> -->
</div>

<style>
    input {
        min-width: 100px;
        margin-right: 20px;
    }
</style>