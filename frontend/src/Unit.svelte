<script>
import { update_await_block_branch } from "svelte/internal";


export let index;
export let unit;
export let update;
export let remove;
export let ranges;
export let priorities;

const latlngStep = 0.1;

</script>

<div class="container">
    <div id="controlsContainer" class="form-group row d-flex justify-content-center align-items-center">
        <label class="col">{`${index + 1}.`}</label>
        <label class="col col-form-label">lat:</label>
        <input type="number" class="col latLngInput" bind:value={unit.position.lat} on:change={() => {update(unit);}} step={latlngStep}/>
        <label class="col col-form-label">lng:</label>
        <input type="number" class="col latLngInput" bind:value={unit.position.lng} on:change={() => {update(unit);}} step={latlngStep}/>
        <button id="removeButton" class="btn btn-danger" on:click={() => {remove(unit);}}>X</button>
    </div>
    {#if unit.priority == 0}
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center spaceAboveAndBelow">
        {#each unit.counts as count, index}
        <input type="number"
                    min={0}
                    bind:value={count}
                    on:change={() => {
                        update(unit);
                    }}
                    class="col countInput"
                    step={1}/>        
        <label class="col col-form-label">{`x ${ranges[index]}km`}</label>
         {/each}
    </div>
    {/if}
    <div class="controlsContainer form-group row d-flex justify-content-center align-items-center littleSpaceAbove">
        <label class="col">{''}</label>
        {#each priorities as priority}
            <button
                class={`col btn selectButton ${unit.priority == priority.priority ? 'btn-success' : 'btn-primary'}`}
                    on:click={() => {
                        unit.priority = priority.priority;
                        if(priority.priority == 0)
                        {
                            unit.counts = [1000, 1000, 1000]; //alert
                        }
                        update(unit);
                        }}>
                <i class={priority.icon}></i>
            </button>
        {/each}
        <label class="col">{''}</label>
        <label class="col">{''}</label>
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
    #removeButton {
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
</style>