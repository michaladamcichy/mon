<script>
import { onMount } from "svelte";
import { validate_component } from "svelte/internal";
import { api } from "../lib/api";
import { test } from '../lib/test.js';

import BottomPaneSection from "./BottomPaneSection.svelte";
import Station from "./Station.svelte";

    export let stations;
    export let units;
    export let stationRanges;
    export let stationCounts;
    export let stationWeights;
    export let updateRanges;
    export let updateCounts;
    export let updateWeights;
    export let updateStations;
    export let updateUnits;
    export let checkIsConnected;
    export let isConnected;

    let oldStationRanges;
    let unitsCount = 50;
    let seed = 0;

    let counts = [1000, 1000, 1000];

    let bigTestRunning = false;
    let bigTestTask;

    onMount(() => {
        oldStationRanges = [...stationRanges];
    });

    const getStationsStats = (_stations) => {
        const stations = [..._stations].filter(station => !station.isStationary);
        const sizes = stations.map(item => item.range);
        console.log(stations);
        let uniqueSizes = [...(new Set(sizes))];
        uniqueSizes.sort();
        let counts = [];
        uniqueSizes.forEach(size => {
            counts.push({range: size, count: sizes.filter(item => item == size).length});
        });
        console.log(counts);
        return counts;
    };

    const onAlgorithmClicked = async (type) => {
        const calculatedStations = await api.algorithm(type, stationRanges, stationCounts, stations, units);
        if(!calculatedStations) {
            console.log('request failed');
            return;
        }
        updateStations(calculatedStations);
    };
</script>

<div class="row">
    <BottomPaneSection title={"Stations' paramaters"}>
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
            <p class="col">ranges</p>
        </div>
        <div class="row">
            {#each stationWeights as weight, index}
                <input type="number"
                    min={0.0}
                    bind:value={weight}
                    on:change={() => {updateCounts(stationCounts);}}
                    class="col"
                    step={0.1}/>
            {/each}
            <p class="col">costs</p>
        </div>
        <div class="row">
            {#each stationCounts as count, index}
                <input type="number"
                    min={0}
                    bind:value={count}
                    class="col"
                    step={1}
                    disabled/>
            {/each}
            <p class="col">counts</p>
        </div>
        
    </BottomPaneSection>
    <BottomPaneSection title={'Algorithms'}>
        <div class="row">
            <button class="col btn btn-primary" on:click={() => onAlgorithmClicked("simpleArrange")}>
                Arrange
            </button>
            <button class="col btn btn-primary" on:click={() => onAlgorithmClicked("arrangeWithExisting")}>
                Arrange with existing
            </button>
        </div>
        <div class="row">
            <button class="col btn btn-primary" on:click={() => onAlgorithmClicked("simpleHierarchicalTree")}>
                Hierarchical tree
            </button>
            <button class="col btn btn-primary" on:click={() => onAlgorithmClicked("priorityArrange")}>
                Priority Arrange
            </button>
        </div>
        <div class="row">
            <button class="col btn btn-primary" on:click={async () => {
                const _unitsCount = unitsCount;
                const positions = test.getRandomUnitsRelated(_unitsCount, seed);
            
                const _units = positions.map(position => {return {position: position, priority: 1}});
                if(_units.length > 0)
                {
                    _units[0].priority = 0;
                    _units[0].counts = [...counts];
                }
                
                const stationaryStations = test.getRandomStationaryStationsRelated(
                            Math.ceil(unitsCount / 10 + 2), seed + 1, _units[0].position.lat, _units[0].position.lng); 
                console.log(stationaryStations);
                const _stations = await api.algorithm('simpleArrange', [...stationRanges], [...counts], stationaryStations, _units);
                //ALERT
                // const _positions = test.getRandomUnitsRelated(Math.floor(_unitsCount / 2), seed);
                // const __units = _positions.map(position => {return {position: position, priority: 1}}); 
                // const __stations = await api.algorithm('arrangeWithExisting', [...stationRanges], [...counts], _stations.concat(stationaryStations), _units.concat(__units));
                
                if(!_stations) {
                    console.log('request failed');
                    return;
                }

                //const isConnected = await api.isConnected([...stationRanges], [...counts], _stations, _units);
            
                updateStations(_stations);
                updateUnits(_units);
            }}>
                Random instance
            </button>
            <input type="number" bind:value={unitsCount} class="col" min='0' step='1'>
            <input type="number" bind:value={seed} class="col" min='0' step='1'>
        </div>
        <div class="row">
            <button class={`col btn ${bigTestRunning ? 'btn-warning' : 'btn-primary'}`} on:click={() => {
                if(bigTestRunning)
                {
                    clearTimeout(bigTestTask);
                    bigTestRunning = false;
                    return;
                }
                bigTestRunning = true;
                // const i = setInterval(f, 500);

                // async function f() {
                //     const positions = test.getRandomUnitsRelated(unitsCount, seed);
                
                //     const newUnits = positions.map(position => {return {position: position, priority: 1}});
                //     if(newUnits.length > 0)
                //     {
                //         newUnits[0].priority = 0;
                //         newUnits[0].counts = [1000, 1000, 1000];
                //     }
                //     await updateUnits(newUnits);
                //     await updateStations([]);

                //     await onAlgorithmClicked('simpleArrange');
                //     const isConnected = await checkIsConnected();
                //     if(isConnected != true) clearInterval(i);
                //}
                const ranges = [...stationRanges];
                const _counts = [...counts];
                bigTestTask = setTimeout(async () => {
                    const _unitsCount = unitsCount;
                    for(let i = 0; i < 1000000; i++)
                    {
                        console.log(i);
                        const positions = test.getRandomUnitsRelated(Math.ceil(_unitsCount / 2), i);
            
                        const _units = positions.map(position => {return {position: position, priority: 1}});
                        if(_units.length > 0)
                        {
                            _units[0].priority = 0;
                            _units[0].counts = [...counts];
                        }

                        const stationaryStations = test.getRandomStationaryStationsRelated(
                            Math.ceil(unitsCount / 10 + 2), i+1, _units[0].position.lat, _units[0].position.lng); 
                        const _stations = await api.algorithm('simpleArrange', ranges, _counts, stationaryStations, _units);
                        if(!_stations) {
                            console.log('request failed');
                            return;
                        }

                        const _positions = test.getRandomUnitsRelated(Math.floor(_unitsCount / 2), i);
                        const __units = _positions.map(position => {return {position: position, priority: 1}}); 
                        const __stations = await api.algorithm('arrangeWithExisting', ranges, _counts, _stations.concat(stationaryStations), _units.concat(__units));
                        const isConnected = await api.isConnected(ranges, counts, __stations.concat(stationaryStations), __units.concat(_units));
                        
                        if(!isConnected || bigTestRunning == false)
                        // if(!.testvalidate(_stations, stationRanges, _units[0].counts) || bigTestRunning == false) //alert counts
                        {
                            updateStations(__stations);
                            updateUnits(_units.concat(__units));
                            bigTestRunning = false;
                            return;
                        }
                    }
                }, 0);

            }}>
                Big test
            </button>
        </div>
        <div class="row">
            {#each counts as count}
                <input bind:value={count} class="col" type="number" min='0' step='1'/>
            {/each}
            <label class="col">counts for test</label>
        </div>
    </BottomPaneSection>
    <BottomPaneSection title={'Status'}>
        <div class="row status">
            {#if isConnected == true}
            <badge class='col btn btn-success'>{'Connected'}</badge>
            {:else if isConnected == false}
            <badge class='col btn btn-danger'>{'Disconnected'}</badge>
            {:else if isConnected == null} 
            <badge class='col btn btn-warning'>{'Server unavailable'}</badge>
            {/if}
        </div>
        <div class="row">
            <div class="col">
                <p><b>{units.length}</b> {' units, '}
                    <b>{stations.filter(station => !station.isStationary).length}</b> {'stations (mobile)'}</p>
            </div>
        </div>
        <div class="row">
        {#each getStationsStats(stations) as stat}
            <div class="col"><p><b>{stat.count}</b> {`x ${stat.range}km `}</p></div>
        {/each}
        </div>
    </BottomPaneSection>
</div>

<style>
    .status {
        margin-bottom: 15px;
    }
    input:disabled
    {
        color: black;
        background-color: lightgray;
    }
</style>