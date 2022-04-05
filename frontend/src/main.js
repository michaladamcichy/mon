import App from './App.svelte';

const app = new App({
	target: document.body,
	props: {
		ready: false,
	}
});

window.initMap = function ready() {
	app.$set({ ready: true });
	console.log('set ready');
};

export default app;