<script>
import { onMount } from "svelte";

    import BottomPaneSection from "./BottomPaneSection.svelte";

    export let stationRanges;
    export let updateRanges;
    export let isConnected;

    let oldStationRanges;

    onMount(() => {
        oldStationRanges = [...stationRanges];
    });
</script>

<div class="row">
    <BottomPaneSection title={"Station ranges (km)"}>
        <div class="row">
            {#each stationRanges as range, index}
                <input type="number"
                    min={index > 0 ? stationRanges[index - 1] + 1 : 0}
                    max={index < stationRanges.length - 1 ? stationRanges[index + 1] - 1 : Infinity}
                    bind:value={range}
                    on:change={() => {updateRanges(stationRanges);}}
                    class="col"
                    step={1}/>
            {/each}
        </div>
    </BottomPaneSection>
    <BottomPaneSection title={''}></BottomPaneSection>
    <BottomPaneSection title={'Status'}>
        <div class="row">
            {#if isConnected == true}
            <badge class='col btn btn-success'>{'Connected'}</badge>
            {:else if isConnected == false}
            <badge class='col btn btn-danger'>{'Disconnected'}</badge>
            {:else if isConnected == null} 
            <badge class='col btn btn-warning'>{'Server unavailable'}</badge>
            {/if}
        </div>
    </BottomPaneSection>
</div>

<style>
    
</style>