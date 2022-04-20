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
        <input type="number" class="col" bind:value={unit.position.lat} on:change={() => {update(unit);}} step={latlngStep}/>
        <label class="col col-form-label">lng:</label>
        <input type="number" class="col" bind:value={unit.position.lng} on:change={() => {update(unit);}} step={latlngStep}/>
        <button class="btn btn-danger" on:click={() => {remove(unit);}}>X</button>
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
    button {
        width: 30px;
        height: 30px;
        font-size: 10px;
        font-weight: bold;
    }
</style>