<script>
import { update_await_block_branch } from "svelte/internal";


export let index;
export let unit;
export let update;
export let remove;
export let ranges;
export let priorities;
export let addUnit;

const latlngStep = 0.1;

</script>

<div class="unit container">
    <div class="form-group row d-flex justify-content-center align-items-center">
        <!-- to align middle nic nie daje -->
        <label class="col col-form-label align-middle">{`${index + 1}. ${unit.master != undefined ? `[${unit.master + 1}]` : ''}`}</label>
        <input type='text col-form-label align-middle' class="col-10 unitName align-middle" bind:value={unit.name} />
    </div>
    <div class="form-group row d-flex justify-content-center align-items-center">
        
        <label class="col col-form-label">lat:</label>
        <input type="number" class="col latLngInput" bind:value={unit.position.lat} on:change={() => {update(unit);}} step={latlngStep}/>
        <label class="col col-form-label">lng:</label>
        <input type="number" class="col latLngInput" bind:value={unit.position.lng} on:change={() => {update(unit);}} step={latlngStep}/>
        <button id="removeButton" class="btn btn-danger col-form-label align-middle" on:click={() => {remove(unit);}}>
            <i class="fa fa-trash"></i></button>
    </div>
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center littleSpaceAbove">
        <label class="col">{''}</label>
        {#each priorities as priority}
            <div
                class={`col btn selectButton ${unit.priority == priority.priority ? 'btn-success' : 'btn-light'}`}
                    on:click={() => {
                        unit.priority = priority.priority;
                        // if(priority.priority == 0)
                        // {
                        //     unit.counts = [1000, 1000, 1000]; //alert
                        // }
                        update(unit);
                        }}>
                <i class={priority.icon}></i>
                    </div>
        {/each}
        <label class="col">{''}</label>
        {#if unit.priority > 0}
        <button class="btn btn-light" on:click={() => {addUnit(unit.priority, index)}}>+</button>
        {:else}
        <label class="col">{''}</label>
        {/if}
        <label class="col">{''}</label>
    </div>  
    <div class="row">
        <br>
    </div>
</div>

<style>
    .latLngInput {
        min-width: 100px;
        margin-right: 20px;
    }
    .countInput {
        text-align: center;
    }
    .selectButton {
        width: 60px;
        /* height: 30px; */
        /* font-size: 5px; */
        font-weight: bold;
    }
    button {
        width: 30px;
        height: 30px;
        font-size: 10px;
        font-weight: bold;
    }
    .miniText {
        font-size: 5px;
    }
    .spaceAboveAndBelow {
        margin-top: 10px;
        margin-left: 10px;
    }

    .littleSpaceAbove {
        margin-top: 5px;
    }

    .unit {
        padding-top: 5px;
        margin-bottom: 5px;
    }

    .unitName {
        background-color: rgba(0,0,0,0);
        margin-bottom: 5px;
        border-color: transparent;
    }
</style>