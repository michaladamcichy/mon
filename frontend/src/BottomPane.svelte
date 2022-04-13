<script>
import { onMount } from "svelte";

    import BottomPaneSection from "./BottomPaneSection.svelte";

    export let stationRanges;
    export let updateRanges;

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
    <BottomPaneSection title={''}></BottomPaneSection>
</div>

<style>
    
</style>