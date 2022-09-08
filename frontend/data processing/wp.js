const API_KEY = 'AIzaSyAGfYUfpBp2kZkQKVw_hq-nHq1q1NhJTwo';

import  fs from 'fs';
import  readline from 'readline';

async function processLineByLine(data, filename) {
  const fileStream = fs.createReadStream('./frontend/data processing/WP/' + filename);

  const rl = readline.createInterface({
    input: fileStream,
    crlfDelay: Infinity
  });
  // Note: we use the crlfDelay option to recognize all instances of CR LF
  // ('\r\n') in input.txt as a single line break.

  for await (const line of rl) {
    // Each line in input.txt will be successively available here as `line`.
    data.push(line);
  }
}




import fetch from "node-fetch";
import { resourceLimits } from "worker_threads";

const textToId = async text => {
    const formattedText = encodeURIComponent(text);
    const request =
`https://maps.googleapis.com/maps/api/place/findplacefromtext/json?key=${API_KEY}&inputtype=textquery&input=${formattedText}`;
    const result = await fetch(request);
    if(!result.ok) return;
    return ((await result.json()).candidates[0].place_id);
};

const idToLocation = async id => {
    const request = `https://maps.googleapis.com/maps/api/place/details/json?place_id=${id}&key=${API_KEY}`;
    const result = await fetch(request);
    if(!result.ok) return;
    return ((await result.json()).result.geometry.location);
};

const nameToLocation = async name => {
    try{
        return await idToLocation(await textToId(name));
    } catch(e) {
        //console.log(e);
        return {lat: 0, lng: 0};
    }
};

const saveFile = async (filename, data) => {
    fs.writeFile(filename, JSON.stringify(data), function(err) {
        if(err) {
            return //console.log(err);
        }
        ////console.log("The file was saved!");
    });
}

const operation = (async (result, priority) => {
    let data = [];
    await processLineByLine(data, priority.toString());
    //console.log(data.length);
    for(let i = 0; i < data.length; i++) {
        const line = data[i];
        const splitted = line.split('-');
        const location = splitted[splitted.length - 1].trim();
        //console.log(location);
        const position = await nameToLocation(location);
        //console.log(position);
        result.push({name: line, position, priority});
    }
});



(async ()  => {
    let result = [];
    for(let i = 0; i <= 4; i++) {
        await operation(result, i);  
    }
    
    saveFile(`wp.json`, result);
})();
