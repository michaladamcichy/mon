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
</script>

<div id="main" class="station container">
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
        <label class="col">{`${index + 1}.`}</label>
        <label class="col col-form-label">lat:</label>
        <input type="number" class="col" bind:value={station.position.lat} on:change={() => {update(station);}} step={latlngStep}/>
        <label class="col col-form-label">lng:</label>
        <input type="number" class="col" bind:value={station.position.lng} on:change={() => {update(station);}} step={latlngStep}/>
        <button id="removeButton" class="btn btn-danger" on:click={() => {remove(station);}}>
            <i class="fa fa-trash"></i></button>
    </div>
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
        <label class="col">{''}</label>
        <label class="col">{'range:'}</label>
        {#if station.isStationary}
            <input type="number" class="col" min='0.1' step = 0.1 default='50.0' bind:value={station.range} on:change={() => {update(station);}} />
        {:else}
        {#each ranges as range}
            <button class={`col btn ${station.range == range ? 'btn-success' : 'btn-light'}`} on:click={() => {station.range = range; update(station);}}>{range}</button>
        {/each}
        {/if}
        <label class="col">{'km'}</label>
        
        <!-- <button class={`col btn btn-${station.isStationary ? 'secondary' : 'primary'}`} on:click={() => {station.groupId = -1; station.isStationary = !station.isStationary; update(station);}}>
            <i class={station.isStationary ? 'fa fa-star' : 'fa fa-truck'}></i>
        </button> -->
        {#if station.groupId >= 0 && !station.isStationary}
        <label class="col">{`g${station.groupId + 1}`}</label>
        {:else}
        <label class="col">{''}</label>
        <label class="col">{''}</label>
        <label class="col">{''}</label>
        {/if}
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
    .station {
        padding-top: 5px;
        padding-bottom: 5px;
        margin-bottom: 5px;
    }
</style>