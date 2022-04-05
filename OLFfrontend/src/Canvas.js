export default class Canvas {
    constructor(canvasElement, pixelWidth, pixelHeight, width, height, gridPrecision) {
        this.canvasElement = canvasElement;
        this.update(canvasElement, pixelWidth, pixelHeight, width, height, gridPrecision);
    }

    update(pixelWidth, pixelHeight, width, height, gridPrecision) {
        console.log(`update ${pixelWidth} x ${pixelHeight}`);
        this.canvasElement.width = pixelWidth;
        this.canvasElement.height = pixelHeight;
        
        if(!height) {
            height = 1.0;
        }
        if(!width) {
            width = pixelWidth / pixelHeight * height;
        }
        if(!gridPrecision) {
            gridPrecision = 0.1;
        }

        this.width = width;
        this.height = height;
        this.gridPrecision = gridPrecision;
        this.unit = pixelHeight;

        this.drawGrid();
    }

    getContext() {
        return this.canvasElement.getContext('2d');
    }

    getDimensions() {
        return this.width, this.height;
    }

    getPixelDimensions() {
        return this.canvasElement.width, this.canvasElement.height;
    }

    getUnit() {
        return this.unit;
    }

    drawGrid() {
        const context = this.getContext();
         
        let i =0;
        this.gridPrecision = 0.;
        for(let y = 0; y < this.height; y += this.gridPrecision) {
            for(let x = 0; x < this.width; x += this.gridPrecision) {
                context.fillStyle = (i++)%2 == 0 ? 'grey' : 'white';
                const _x = x * this.gridPrecision * this.unit;
                const _y = y * this.gridPrecision * this.unit;
                const _w = this.width * this.unit;
                const _h = this.height * this.unit;

                context.fillRect(_x, _y, _w, _h);
            }
        }
    }
}