<script>
import { onMount } from "svelte";
import { api } from "../lib/api";
import { test } from '../lib/test.js';

import BottomPaneSection from "./BottomPaneSection.svelte";
import Station from "./Station.svelte";

    export let stations;
    export let units;
    export let ranges;
    export let stationCounts;
    export let updateCounts;
    export let updateRanges;
    export let updateStations;
    export let updateUnits;
    export let checkIsConnected;
    export let isConnected;
    export let bigTestRunning;
    export let updateBigTestRunning;
    export let percentageOfStations;
    export let updatePercentage;
    export let loadGSM;
    export let disableMap;
    export let updateDisableMap;
    export let optimized;
    export let updateOptimized;

    let counts = [...stationCounts];

    let unitsCount = 50;
    let seed = 0;

    let bigTestTask;

    let selectAlgorithm = 'simpleArrange';
    let lastOperation = {name: null, time: null};
    
    //alert
    let lastI = 0;

    onMount(() => {
        console.log(ranges);
        ranges = [...ranges];
    });

    const getStationsStats = (_stations) => {
        const stations = [..._stations].filter(station => !station.isStationary);
        const sizes = stations.map(item => item.range);
        //console.log(stations);
        let uniqueSizes = [...(new Set(sizes))];
        uniqueSizes.sort();
        let counts = [];
        uniqueSizes.forEach(size => {
            counts.push({range: size, count: sizes.filter(item => item == size).length});
        });

        return counts;
    };

    const getStationsScore = stations => stations.filter(station => !station.isStationary).reduce((current, station) => current + station.range, 0);

    const onAlgorithmClicked = async (type) => {
        const res = await api.algorithm(type, ranges, stationCounts, stations, units, optimized);
        if(!res) {
            //console.log('request failed');
            return;
        }
        lastOperation = {name: type, time: res.milliseconds};
        updateStations(res.stations);
        
        let text = res.milliseconds.toString() + 'ms |' + res.stations.filter(item => !item.isStationary).length.toString() + ' ' + getStationsScore(res.stations).toString() + ' ';
        getStationsStats(res.stations).forEach(item => text += item.count.toString() + 'x' + item.range + ' ');
        console.log(text);
    };
</script>
<div class="row">
    <BottomPaneSection title={"Stations' parameters"}>
        <div class="row">
            <h5 class="col">Ranges (km)</h5>
        </div>
        <div class="row">
            {#each ranges as range, index}
                <input type="number"
                    min={index > 0 ? ranges[index - 1] + 1 : 0}
                    max={index < ranges.length - 1 ? ranges[index + 1] - 1 : Infinity}
                    bind:value={range}
                    on:change={() => {updateRanges(ranges);}}
                    class="col"
                    step={1} />
            {/each}
        </div>
        <!-- <div class="row">
            {#each stationWeights as weight, index}
                <input type="number"
                    min={0.0}
                    bind:value={weight}
                    on:change={() => {updateCounts(stationCounts);}}
                    class="col"
                    step={0.1} disabled/>
            {/each}
            <p class="col">costs</p>
        </div> -->
        <div class="row">
            <div class="col" style={'visibility: hidden'}>{'_'}</div>
        </div>
        <div class="row">
            <h5 class="col">Counts</h5>
        </div>
        <div class="row">
            {#each counts as count, index}
                <input type="number"
                    min={0}
                    bind:value={count}
                    on:change={() => {updateCounts(counts);}}
                    class="col"
                    step={1}
                    />
            {/each}
        </div>
        <!-- <div class="row">
            <input class="col" type="number" min='0' max="100" step='10' bind:value={percentageOfStations}
                on:change={(e) => {updatePercentage(e.target.value); loadGSM(e.target.value);}}>
            <label class="col">% of stationary stations</label>
        </div> -->
        
    </BottomPaneSection>
    <BottomPaneSection title={'Algorithms'}>
        <div class="row">
            <button class="col btn btn-light algorithmButton" on:click={() => onAlgorithmClicked("naiveArrange")}>
                Naive
            </button>
            <button class="col btn btn-light algorithmButton" on:click={() => onAlgorithmClicked("simpleArrange")}>
                Rationalised
            </button>
            <!-- <input class="col" type="checkbox" bind:checked={optimized} on:change={() => {updateOptimized(optimized);}}> -->
        </div>
        <div class="row">
            <button class="col btn btn-light algorithmButton" on:click={() => onAlgorithmClicked("arrangeWithExisting")}>
                Add to existing
            </button>
            <button class="col btn btn-light algorithmButton" on:click={() => onAlgorithmClicked("priorityArrange")}>
                Priority based
            </button>
        </div>
        <!-- <div class="row">
            <select bind:value={selectAlgorithm}>
                <option value="simpleArrange"> Arrange </option>
                <option value="arrangeWithExisting"> With existing </option>
                <option value="priorityArrange"> Priority arrange </option>
            </select>
        </div> -->
        <div class="row">
            <button class="col btn btn-light algorithmButton" on:click={() => onAlgorithmClicked("simpleOptimize")}>
                Optimize
            </button>
        </div>
        
        <div class="row">
            <button class="col btn btn-light algorithmButton" on:click={async () => {
                updatePercentage(0);
                const positions = test.getRandomUnitsRelated(unitsCount, seed);
            
                // let ____i = 0;
                const _units = positions.map(position => {return {position: position, priority: positions.indexOf(position)%5}});
                if(_units.length > 0)
                {
                    _units[0].priority = 0;
                    _units[0].counts = [...counts];
                }
                const stationaryStations = [];//test.getRandomStationaryStationsRelated(Math.ceil(unitsCount /*/ 10 + 2*/), seed + 1, _units[0].position.lat, _units[0].position.lng); 
                
                updateUnits(units.concat(_units));
            }}>
                Generate random units
            </button>
            <input type="number" bind:value={unitsCount} class="col-4 randomUnitsCountInput" min='0' step='1'>
            <!-- <input type="number" bind:value={lastI} class="col" min='0' step='1'> -->
        </div>
        <!-- <div class="row">
            <button class={`col btn ${bigTestRunning ? 'btn-warning' : 'btn-light'}`} on:click={() => {
                if(bigTestRunning)
                {
                    clearTimeout(bigTestTask);
                    updateBigTestRunning(false);
                    return;
                }
                updateBigTestRunning(true);
                //console.log(selectAlgorithm);
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
                const ranges = [...ranges];
                const _counts = [...counts];
                bigTestTask = setTimeout(async () => {
                    for(let i = lastI; i < 1000000; i++)
                    {
                        console.log(i);
                        const positions = test.getRandomUnitsRelated(Math.ceil(unitsCount), i);
            
                        let _i = 0;
                        const _units = positions.map(position => {return {position: position, priority: ((_i++)%5)}});
                        if(_units.length > 0)
                        {
                            _units[0].priority = 0;
                            _units[0].counts = [...counts];
                        }

                        const stationaryStations = [];//test.getRandomStationaryStationsRelated(
                            //Math.ceil(unitsCount /* / 10 + 2*/), i+1, _units[0].position.lat, _units[0].position.lng); 
                        const res = await api.algorithm(selectAlgorithm, ranges, _counts, stationaryStations, _units, optimized);
                        if(!res) {
                            //console.log('request failed');
                            return;
                        }
                        const _stations = res.stations;
                        const _positions =[];//test.getRandomUnitsRelated(Math.floor(_unitsCount / 2), i);
                        let priority = 1;
                        const __units = _positions.map(position => {return {position: position, priority: (priority++)%4 + 1}}); 
                        //const __res = await api.algorithm('arrangeWithExisting', ranges, _counts, _stations.concat(stationaryStations), _units.concat(__units));
                        const __stations = _stations;//__res.stations;
                        const isConnected = await api.isConnected(ranges, counts, __stations.concat(stationaryStations), __units.concat(_units));
                        
                        lastI = i+1;
                        if(!isConnected || !test.validate(__stations, ranges, _units[0].counts) || bigTestRunning == false)
                        {
                            updateStations(__stations);
                            updateUnits(_units.concat(__units));
                            updateBigTestRunning(false);
                            return;
                        }    
                    }
                    updateBigTestRunning(false);
                    return;
                }, 0);

            }}>
                Big test
            </button>
        </div> -->
        <!-- <div class="row">
            {#each counts as count}
                <input bind:value={count} class="col" type="number" min='0' step='1'/>
            {/each}
            <label class="col">counts for test</label>
        </div> -->
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
            <!-- <div class="col"> -->
                <p class="col"><b>{units.length}</b> {' units, '}
                    <b>{stations.filter(station => !station.isStationary).length}</b> {'stations'}
                    <!-- <b>{stations.filter(station => station.isStationary).length}</b> {'stationary'} -->
                </p>
            <!-- </div> -->
        </div>
        <div class="row">
            <p class="col">Total cost:</p>
            <p class="col">
                <b>{`(${stations.filter(station => !station.IsStationary).length}, ${getStationsScore(stations)})`}</b></p>
        </div>
        <div class="row">
        {#each getStationsStats(stations) as stat}
            <div class="col"><p><b>{stat.count}</b> {`x ${stat.range}km `}</p></div>
        {/each}
        </div>
        {#if lastOperation.name}
        <div class="row">
            <p class="col">{`Last operation: `} <b>{`${lastOperation.name}`} </b></p>
        </div>
        <div class="row">  
            <p class="col">{`Completed within: `} <b>{`${lastOperation.time / 1000} s`} </b></p>
        </div>
        {/if}
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
    .algorithmButton {
        border-radius: 40px;
        margin: 5px;
    }
    .randomUnitsCountInput {
        text-align: center;
        border-radius: 20px;
    }
</style>
