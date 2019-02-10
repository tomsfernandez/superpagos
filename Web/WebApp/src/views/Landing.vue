<template>
	<div class="landing">
		<div class="col-sm-6 superpagos-background">
			<h1 class="superpagos-title">{{title}}</h1>
			<img class="superpagos-logo" src="../assets/landing-image.svg" alt="Superpagos Logo">
		</div>
		<router-view></router-view>
	</div>
</template>

<script>

	export default {
		name: "Landing",
		data() {
			return {
				title: "Superpagos"
			}
		}
	}
</script>

<style scoped>
	.landing {
		height: 100%;
		display: flex;
	}

	.superpagos-background {
		background-color: #fee444;
		overflow: hidden;
	}

	.superpagos-title {
		position: relative;
		text-align: center;
		top: 30%;
		color: white;
		font-size: 6vw;
		text-shadow: 0 2px 5px black, 0 1px 10px black;
		z-index: 1;
	}

	.superpagos-logo {
		position: absolute;
		top: 20%;
	}
</style>