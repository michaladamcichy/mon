export const test = {};

const startPosition = {lat: 52.2297, lng: 21.0122};
const normalize = (lat, lng) => {
    const divider = Math.sqrt(lat*lat + lng*lng);
    return {lat: lat / divider, lng: lng / divider}; 
};

const multiplier = 1000;

const rand = (seed) => {
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

    console.log(`Seed: ${seed}`);

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
        units.push({lat: units[i-1].lat + direction.lat * distance, lng: units[i-1].lng + direction.lng * distance});
    }

    console.log(`Seed: ${seed}`);

    return units;
};


test.validate = (stations, ranges, counts) => {
    for(let i = 0; i < ranges.length; i++)
    {
        if(stations.filter(station => station.range == ranges[i]).length > counts[i]) return false;
    }
    return true;
};