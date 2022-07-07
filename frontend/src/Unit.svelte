<script>
import { update_await_block_branch } from "svelte/internal";


export let index;
export let unit;
export let update;
export let remove;

const latlngStep = 0.1;

const priorities = [
    {priority: 4, icon: 'fa fa-exclamation'},
    {priority: 3, icon: 'fa fa-truck'},
    {priority: 2, icon: 'fa fa-star'},
    {priority: 1, icon: 'fa fa-male'},
    {priority: 0, icon: 'fa fa-bitbucket'},
    ];
</script>

<div class="container">
    <div id="controlsContainer" class="form-group row d-flex justify-content-center align-items-center">
        <label class="col">{`${index + 1}.`}</label>
        <label class="col col-form-label">lat:</label>
        <input type="number" class="col" bind:value={unit.position.lat} on:change={() => {update(unit);}} step={latlngStep}/>
        <label class="col col-form-label">lng:</label>
        <input type="number" class="col" bind:value={unit.position.lng} on:change={() => {update(unit);}} step={latlngStep}/>
        <button id="removeButton" class="btn btn-danger" on:click={() => {remove(unit);}}>X</button>
    </div>
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center">
        <label class="col">{''}</label>
        {#each priorities as priority}
            {#if priority.priority == 0}
                <div class="col"></div>
            {/if}
            <button
                class={`col btn selectButton ${unit.priority == priority.priority ? 'btn-success' : 'btn-primary'}`}
                    on:click={() => {unit.priority = priority.priority; update(unit);}}>
                <i class={priority.icon}></i>
            </button>
        {/each}
        <label class="col">{''}</label>
        <label class="col">{''}</label>
        <label class="col">{''}</label>
    </div>
</div>

<style>
    input {
        min-width: 100px;
        margin-right: 20px;
    }
    .selectButton {
        width: 60px;
        /* height: 30px; */
        font-size: 20px;
        font-weight: bold;
    }
    #removeButton {
        width: 30px;
        height: 30px;
        font-size: 10px;
        font-weight: bold;
    }
</style>