<script>
import { onMount } from "svelte";

import Canvas from './Canvas.js';


    let innerWidth;
    let innerHeight;
    let outerWidth;
    let outerHeight;

    $: {
        if(canvas) {
            canvas.update(innerWidth, innerHeight);
        }
    }

    let canvasElement;
    let canvas;

    onMount(() => {
        canvas = new Canvas(canvasElement, innerWidth, innerHeight);
    });

    
    // canvas.height = innerHeight;
    // canvas.width = innerWidth;
</script>

<svelte:window
    bind:innerWidth={innerWidth}
    bind:innerHeight={innerHeight}
    bind:outerWidth={outerWidth}
    bind:outerHeight={outerHeight}
/>

<canvas bind:this={canvasElement} id='mainCanvas'></canvas>

<style>
    #mainCanvas {
        border: 2px solid black;
    }
</style>