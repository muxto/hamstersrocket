import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router';
import PageReport from '../views/PageReport/index.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Report',
    component: PageReport,
  },
  {
    path: '/ticker/:id',
    name: 'PageTicker',
    // route level code-splitting
    // this generates a separate chunk (ticker.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "ticker" */ '../views/PageTicker.vue'),
  },
  {
    path: '/about',
    name: 'PageAbout',
    // route level code-splitting
    // this generates a separate chunk (ticker.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/PageAbout/index.vue'),
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
