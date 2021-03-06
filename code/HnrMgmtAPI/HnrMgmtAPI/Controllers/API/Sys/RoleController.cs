﻿using HnrMgmtAPI.Common;
using HnrMgmtAPI.Models;
using HnrMgmtAPI.Models.API;
using HnrMgmtAPI.Models.API.Sys;
using System.Linq;
using System.Web.Http;

namespace HnrMgmtAPI.Controllers.API.Sys
{
    [RoutePrefix("api/role")]
    public class RoleController : BaseApiController
    {
        #region API接口管理（用户可以自行手动添加）
        /// <summary>
        /// 获取功能列表  返回全部功能列表
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        [HttpGet, Route("getmenu")]
        public ApiResult GetMenu(string access_token)
        {
            result = AccessToken.Check(access_token, "api/role/getmenu");
            if (result == null)
            {
                #region 逻辑操作
                var menuList = from T_Right in db.T_Right orderby T_Right.Priority select T_Right;
                if (menuList.Any())
                {
                    Return_GetList<T_Right> right_list = new Return_GetList<T_Right>();
                    right_list.list = menuList.ToList();
                    right_list.count = menuList.Count();
                    return Success("获取功能列表成功", right_list);
                }
                else
                {
                    return Error("获取失败，功能列表为空");
                }
                #endregion
            }
            return result;
        }

        /// <summary>
        /// 添加功能列表 
        /// API接口功能列表中 0代表系统管理级  1代表普通操作级
        /// 此处允许用户添加的是  普通操作级  [Priority]值均为1
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="MenuName"></param>
        /// <param name="MenuUrl"></param>
        /// <returns></returns>
        [HttpPost, Route("addmenu")]
        public ApiResult AddMenu([FromBody]MenuAdd model)
        {
            result = AccessToken.Check(model.access_token, "api/role/addmenu");

            if (result == null)
            {
                #region 参数验证
                result = ParameterCheck.CheckParameters(model);
                if (result != null)
                {
                    return result;
                }
                #endregion

                #region 逻辑操作
                var menulist = from T_Right in db.T_Right where (T_Right.Url == model.Url) select T_Right;
                if (menulist.Any())
                {
                    //已存在
                    return Error("此API功能接口已存在");
                }
                else
                {
                    T_Right rightModel = new T_Right();
                    rightModel.ID = System.Guid.NewGuid().ToString();
                    rightModel.Name = model.Name;
                    rightModel.Url = model.Url;
                    rightModel.Priority = 1;
                    try
                    {
                        db.T_Right.Add(rightModel);
                        db.SaveChanges();

                        return Success("添加成功");
                    }
                    catch
                    {
                        return Error("添加失败");
                    }
                }
                #endregion
            }

            return result;
        }

        /// <summary>
        /// 修改功能表
        /// </summary>
        /// <param name="model">参数参考MenuModify</param>
        /// <returns></returns>
        [HttpPost, Route("modmenu")]
        public ApiResult ModifyMenu([FromBody]MenuModify model)
        {
            result = AccessToken.Check(model.access_token, "api/role/modmenu");
            if (result == null)
            {
                #region 参数验证
                result = ParameterCheck.CheckParameters(model);
                if (result != null)
                {
                    return result;
                }
                #endregion

                #region 逻辑操作
                T_Right rightModel = db.T_Right.Find(model.ID);
                if (rightModel != null)
                {
                    if (rightModel.Priority == 0)
                    {
                        return Error("此记录不能被修改");
                    }

                    rightModel.Name = model.Name;
                    rightModel.Url = model.Url;

                    try
                    {
                        db.SaveChanges();

                        return Success("修改成功");
                    }
                    catch
                    {
                        return Error("修改失败");
                    }
                }
                else
                {
                    return Error("未找到此记录");
                }
                #endregion
            }
            return result;
        }

        /// <summary>
        /// 删除功能表中记录
        /// </summary>
        /// <param name="access_token">授权令牌</param>
        /// <param name="ID">记录ID</param>
        /// <returns></returns>
        [HttpGet, Route("delmenu")]
        public ApiResult DeleteMenu(string access_token, string ID)
        {
            result = AccessToken.Check(access_token, "api/role/delmenu");
            if (result == null)
            {
                #region 参数验证
                if (ID == null || ID == "")
                {
                    return Error("参数错误，ID不能为空");
                }
                #endregion

                #region 逻辑操作
                T_Right rightModel = db.T_Right.Find(ID);
                if (rightModel != null)
                {
                    var rightMenuList = from vw_Role in db.vw_Role where (vw_Role.MenuID == ID) select vw_Role;
                    if (rightMenuList.Any())
                    {
                        return Error("此接口已被使用，不能被删除");
                    }

                    try
                    {
                        db.T_Right.Remove(rightModel);
                        db.SaveChanges();

                        return Success("删除成功");
                    }
                    catch
                    {
                        return Error("删除失败");
                    }
                }
                else
                {
                    return Error("未找到此记录");
                }
                #endregion
            }
            return result;
        }
        #endregion

        #region 角色管理 （不对用户开放、数据库中设置用户层级、不允许用户修改）
        /// <summary>
        /// 获取系统中的角色列表，不允许用户添加、删除等操作
        /// </summary>
        /// <param name="access_token">授权令牌</param>
        /// <returns></returns>
        [HttpGet, Route("getrole")]
        public ApiResult GetRole(string access_token)
        {
            result = AccessToken.Check(access_token, "api/role/getrole");
            if (result == null)
            {
                #region 逻辑操作
                var roleList = from T_Role in db.T_Role orderby T_Role.RoleID select T_Role;
                if (roleList.Any())
                {
                    Return_GetList<T_Role> role_list = new Return_GetList<T_Role>();
                    role_list.list = roleList.ToList();
                    role_list.count = roleList.Count();
                    return Success("获取角色列表成功", role_list);
                }
                else
                {
                    return Error("获取失败，角色列表为空");
                }
                #endregion
            }
            return result;
        }
        #endregion

        #region 角色设定功能管理（用户可根据需要为某种角色设定可执行的操作）
        /// <summary>
        /// 获取角色功能列表
        /// </summary>
        /// <param name="access_token">授权令牌</param>
        /// <returns></returns>
        [HttpGet, Route("get")]
        public ApiResult GetRoleMenu(string access_token)
        {
            result = AccessToken.Check(access_token, "api/role/get");
            if (result == null)
            {
                #region 参数验证
                #endregion

                #region 逻辑操作
                var roleList = from T_Role in db.T_Role orderby T_Role.RoleID select T_Role;
                if (roleList.Any())
                {
                    //定义返回 数据 Data
                    Return_GetList<RoleMenuModel> data = new Return_GetList<RoleMenuModel>();
                    data.count = roleList.Count();
                    data.list = new System.Collections.Generic.List<RoleMenuModel>();

                    //遍历角色列表
                    foreach (var item in roleList.ToList())
                    {
                        RoleMenuModel roleMenuModel = new RoleMenuModel();
                        roleMenuModel.RoleID = item.RoleID;
                        roleMenuModel.RoleName = item.Name;
                        var menuList = from vw_Role in db.vw_Role where (vw_Role.RoleID == item.RoleID) select vw_Role;
                        if (menuList.Any())
                        {
                            roleMenuModel.count = menuList.Count();
                            foreach (var _item in menuList.ToList())
                            {
                                MenuModel menuModel = new MenuModel();
                                menuModel.ApiID = _item.MenuID;
                                menuModel.ApiName = _item.MenuName;
                                menuModel.ApiUrl = _item.Url;

                                roleMenuModel.MenuList.Add(menuModel);
                            }
                        }
                        else
                        {
                            roleMenuModel.count = 0;
                            roleMenuModel.MenuList = null;
                        }

                        data.list.Add(roleMenuModel);
                    }

                    return Success("获取角色功能列表", data);
                }
                else
                {
                    return Error("尚未设定角色列表");
                }
                #endregion
            }
            return result;
        }

        /// <summary>
        /// 设置角色功能
        /// </summary>
        /// <param name="model">参数参考 RoleMenuSet</param>
        /// <returns></returns>
        [HttpPost, Route("set")]
        public ApiResult SetRoleMenu([FromBody]RoleMenuSet model)
        {
            return Error("接口尚未实现");
            //result = AccessToken.Check(model.access_token, "api/role/set");
            //if (result == null)
            //{
            //    #region 参数验证
            //    result = ParameterCheck.CheckParameters(model);
            //    if (result != null)
            //    {
            //        return result;
            //    }
            //    #endregion

            //    #region 逻辑操作
            //    if (model.MenuList != null && model.MenuList.Count > 0)
            //    {

            //    }
            //    #endregion
            //}
            //return result;
        }
        #endregion
    }
}
