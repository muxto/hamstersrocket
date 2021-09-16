import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router';
import PageReport from '../views/PageReport.vue';

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
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/PageTicker.vue'),
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
