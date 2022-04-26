<script>
import { onMount } from "svelte";
import { api } from "../lib/api";

    import BottomPaneSection from "./BottomPaneSection.svelte";

    export let stations;
    export let units;
    export let stationRanges;
    export let stationCounts;
    export let updateRanges;
    export let updateStations;
    export let isConnected;

    let oldStationRanges;

    onMount(() => {
        oldStationRanges = [...stationRanges];
    });

    const onSimpleArrangeAlgorithmClicked = async () => {
        const calculatedStations = await api.simpleArrangeAlgorithm(stationRanges, stationCounts, stations, units);
        if(!calculatedStations) {
            console.log('request failed');
            return;
        }
        updateStations(calculatedStations);
    };
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
    <BottomPaneSection title={'Algorithms'}>
        <div class="row">
            <button class="btn btn-primary" on:click={() => onSimpleArrangeAlgorithmClicked()}>
                Simple arrange algorithm
            </button>
        </div>
    </BottomPaneSection>
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