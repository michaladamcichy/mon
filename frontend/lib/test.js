import {api} from './api.js';
export const test = {};

const startPosition = {lat: 52.2297, lng: 21.0122};
const normalize = (lat, lng) => {
    const divider = Math.sqrt(lat*lat + lng*lng);
    return {lat: lat / divider, lng: lng / divider}; 
};

const multiplier = 1000;

const rand = (seed) => {
    //aLert!!!
    return Math.random();
    var x = Math.sin(seed) * 10000;
    return x - Math.floor(x);
};

const randomDirection = () => {
    return normalize({lat: rand() * 2 - 1, lng: rand() * 2 - 1});
};

const randomPosition = () => {
  //alert
  throw null;  
};

test.createRandomInstance = (unitsCount) => {
};

// Test.getRandomUnits = (unitsCount) => {
//     let units = [];

//     let lastPosition = {...startPosition};
//     for(let i = 0; i < unitsCount; i++)
//     {
//         direction = randomDirection();
//         distance = rand() * multiplier;
//         units.push({lat: lastPosition.lat + randomDirection.lat * distance, lng: lastPosition.lng + randomDirection.lng * distance});
//     }
// };

const latMin = 49;
const latMax = 54;
const lngMin = 14;
const lngMax = 24;

const randomLat = (seed) => {
    return latMin + rand(seed) * (latMax - latMin);
};

const randomLng = (seed) => {
    return lngMin + rand(seed) * (lngMax - lngMin);
};

test.getRandomUnitsRandom = (unitsCount, seed) => {
    let units = [];

    seed = seed ? seed : Math.floor(Math.random() * 10000000);

    for(let i = 0; i < unitsCount; i++)
    {
        units.push({lat: randomLat(seed++), lng: randomLng(seed++)});
    }

    //console.log(`Seed: ${seed}`);

    return units;
};

test.getRandomUnitsRelated = (unitsCount, seed) => {
    let units = [];

    seed = seed ? seed : Math.floor(Math.random() * 10000000);

    units.push({lat: randomLat(seed++), lng: randomLng(seed++)});

    for(let i = 1; i < unitsCount; i++)
    {
        let direction = normalize(rand(seed++) * 2 - 1.0, rand(seed++) * 2 - 1.0);
        let distance = rand(seed++) * 1;
        units.push({lat: units[i-1].lat + direction.lat * distance, 
            lng: units[i-1].lng + direction.lng * distance,
        });
    }

    return units;
};

test.getRandomStationaryStationsRelated = (stationsCount, seed, lat, lng) => {
    let stations = [];

    seed = seed ? seed : Math.floor(Math.random() * 10000000);

    stations.push({
        position:
                {
                    lat: lat + 0.1,
                    lng: lng + 0.1,
                },
            range: 50.0,
            isStationary: true
    });

    for(let i = 1; i < stationsCount; i++)
    {
        let direction = normalize(rand(seed++) * 2 - 1.0, rand(seed++) * 2 - 1.0);
        let distance = rand(seed++) * 1; //alert
        stations.push({
            position:
                {
                    lat: stations[i-1].position.lat + direction.lat * distance,
                    lng: stations[i-1].position.lng + direction.lng * distance,
                },
            range: 50.0,
            isStationary: true
        });
    }

    return stations;
};


test.validate = (stations, ranges, counts) => {
    for(let i = 0; i < ranges.length; i++)
    {
        if(stations.filter(station => station.range == ranges[i]).length > counts[i]) return false;
    }
    return true;
};
////////////////////

const printTimes = seriess => {
    console.log('TIMES');
    const N = Object.keys(seriess[0]);
    N.forEach(n => {
        let text = `${n.toString()}`;
        seriess.forEach(series => {
            const avgTime = Math.round(series[n].reduce((cur, next) => cur + next, 0) / series[n].length * 100.0) / 100;
            const timeDev = Math.round(Math.sqrt(series[n].reduce((cur, next) => cur + Math.pow(next - avgTime,2), 0) / (series[n].length - 1)) * 100.0) / 100;
            text += ` & ${avgTime} & ${timeDev}`;
        });
        text += '\\\\\n';

        console.log(text);
    });
};

const getStationsScore = stations => stations.filter(station => !station.isStationary).reduce((current, station) => current + station.range, 0);


const printCosts = seriess => {
    console.log('COSTS');
    const N = Object.keys(seriess[0]);
    //console.log(seriess);
    N.forEach(n => {
        let text = `${n.toString()}`;
        seriess.forEach(series => {
            const counts = series[n].map(item => item.length);
            const costs = series[n].map(item => getStationsScore(item));
            
            const avgStationsCount = Math.round(counts.reduce((cur, next) => cur + next, 0) / counts.length * 100.0) / 100;
            const stationsCountDev = Math.round(Math.sqrt(counts.reduce((cur, next) => cur + Math.pow(next - avgStationsCount,2), 0) / (counts.length - 1)) * 100.0) / 100;
            
            const avgCost = Math.round(costs.reduce((cur, next) => cur + next, 0) / (costs.length) * 100.0) / 100;
            const costDev = Math.round(Math.sqrt(costs.reduce((cur, next) => cur + Math.pow(next - avgCost,2), 0) / (costs.length - 1)) * 100.0) / 100;
            
            
            text += ` & (${avgStationsCount}, ${avgCost}) & (${stationsCountDev}, ${costDev})`;
        });
        text += '\\\\\n';

        console.log(text);
    });
};

// let text = res.milliseconds.toString() + 'ms |' + res.stations.filter(item => !item.isStationary).length.toString() + ' ' + getStationsScore(res.stations).toString() + ' ';
//         getStationsStats(res.stations).forEach(item => text += item.count.toString() + 'x' + item.range + ' ');
//         console.log(text);

const algorithm = async (algorithm, ranges, counts, stations, units) => 
{
    let res = await api.algorithm(algorithm, ranges, counts, stations, units);
    
    return {stations: res.stations, time: res.milliseconds};
}

const naiveVsSimple = async (N, k, ranges, counts) => {
    console.log('naiveSimple');
    
    const naiveTimes = {};
    const naiveStations = {};
    const simpleTimes = {};
    const simpleStations = {};

    for(let i = 0; i < N.length; i++)
    {
        const n = N[i];
        console.log(n);
        naiveTimes[n] = [];
        naiveStations[n] = [];
        simpleTimes[n] = [];
        simpleStations[n] = [];

        for(let i = 0; i < k; i++) {
            const units = test.getRandomUnitsRelated(n).map(position => {return {position: position, priority: 1}});;
            //console.log(units);
            const naiveResults = await algorithm("naiveArrange", ranges, counts,  [], units);
            const simpleResults = await algorithm("simpleArrange", ranges, counts, [], units);
            //console.log(naiveResults.stations);
            //console.log(simpleResults.stations);
            naiveTimes[n].push(naiveResults.time);
            naiveStations[n].push(naiveResults.stations);
            simpleTimes[n].push(simpleResults.time);
            simpleStations[n].push(simpleResults.stations);
        }
    }
    printTimes([naiveTimes, simpleTimes]);
    printCosts([naiveStations, simpleStations]);
};

test.run = async () => {
    console.log('TEST');
    await naiveVsSimple([50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000], 10, [20, 30, 50], [10000,10000,10000]);
    // await naiveVsSimple([50], 4, [20, 30, 50], [10000,10000,10000]);
};
