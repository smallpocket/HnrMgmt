import Vue from 'vue'
import Router from 'vue-router'
import Home from '../views/Home.vue'
import NotFoundComponent from '../views/404'
import Hello from '@/components/Hello'

Vue.use(Router)

// 定义路由
const routes = [
//   {

//   // path: '/',
//   // redirect: {
//   //   name: 'Home'
//   // }
// },
// 404页面
  {
    path: '*',
    component: NotFoundComponent
  },
  {
    path: '/',
    name: 'Home',
    component: Home,
    redirect: '/index',
    children: [
      {path: '/index', component: Hello, name: 'index', menuShow: true}
    ]
  }
]

// 暴露路由
export default new Router({
  mode: 'history',
  routes: routes
})

// export default new Router({
//   routes: [
//     {
//       path: '/',
//       name: 'Hello',
//       component: Hello
//     }
//   ]
// })
