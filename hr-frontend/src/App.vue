<template>
  <!-- <div id="nav">
    <router-link to="/">Report</router-link> |
    <router-link to="/ticker/123">Single Ticker</router-link>
  </div> -->
  <nav class="navbar navbar-expand-lg navbar-light bg-light">
    <div class="container">
      <router-link class="navbar-brand" to="/">Hamster's Rocket</router-link>
      <button
        class="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#navbarSupportedContent"
        aria-controls="navbarSupportedContent"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
          <li class="nav-item">
            <router-link
              v-slot="{ isActive }"
              :class="[
                'nav-link',
                isActive && 'active',
              ]" to="/about">О нас</router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
  <router-view/>
</template>
