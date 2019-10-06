/* eslint-disable */
import util from './libs/util.ice'; // 菜单配置
// 侧栏菜单配置
// ice 会在新建页面的时候 push 数据
// ice 自动添加的菜单记录是以下格式：(不会有嵌套)
// {
//   name: 'Nav',
//   path: '/page',
//   icon: 'home',
// },

const asideMenuConfig = [
  {
    name: '文章管理',
    icon: 'book',
    children: [
      {
        name: '新增文章',
        path: '/ArticleEdit/',
        icon: 'file-text',
      },
      {
        name: '文章列表',
        path: '/Article/',
        icon: 'table',
      },
    ],
  },
  {
    name: '会员管理',
    icon: 'user',
    children: [
      {
        name: '新增会员',
        path: '/MemberEdit/',
        icon: 'user-plus',
      },
      {
        name: '会员列表',
        path: '/Member/',
        icon: 'users',
      },
    ],
  },
  {
    name: '评论管理',
    icon: 'comment',
    children: [
      {
        name: '评论列表',
        path: '/Comment/',
        icon: 'comments',
      },
    ],
  },
  {
    name:'留言管理',
    icon:'pencil',
    children:[
      {
      name: '留言列表',
      path: '/Message/',
      icon: 'commenting-o',
      },
  ]
  },
  {
    name: '文件管理',
    icon: 'download',
    children: [
      {
        name: '新增文件',
        path: '/FileEdit/',
        icon: 'jsfiddle',
      },
      {
        name: '文件列表',
        path: '/File/',
        icon: 'list-alt',
      },
    ],
  },
  {
    name: '系统设置',
    icon: 'cogs',
    children: [
      {
        name: '网站设置',
        path: '/SiteSetting/',
        icon: 'sliders',
      },
      {
        name: '邮箱配置',
        path: '/EmailSetting/',
        icon: 'envelope',
      },
      {
        name: '短信配置',
        path: '/SmsSetting/',
        icon: 'envelope-square',
      },
      {
        name: '网站状态',
        path: '/',
        icon: 'tachometer',
      },
    ],
  },
]; // 顶栏菜单配置
// ice 不会修改 headerMenuConfig
// 如果你需要功能开发之前就配置出菜单原型，可以只设置 name 字段
// D2Admin 会自动添加不重复 id 生成菜单，并在点击时提示这是一个临时菜单

const headerMenuConfig = [
  {
    name: '首页',
    icon: 'home',
    path: '/',
  },
]; // 请根据自身业务逻辑修改导出设置，并在合适的位置赋给对应的菜单
// 参考
// 设置顶栏菜单的方法 (vuex)
// $store.commit('d2adminMenuHeaderSet', menus)
// 设置侧边栏菜单的方法 (vuex)
// $store.commit('d2adminMenuAsideSet', menus)
// 你可以在任何地方使用上述方法修改顶栏和侧边栏菜单
// 导出顶栏菜单

export const menuHeader = util.recursiveMenuConfig(headerMenuConfig); // 导出侧边栏菜单

export const menuAside = util.recursiveMenuConfig(asideMenuConfig);
