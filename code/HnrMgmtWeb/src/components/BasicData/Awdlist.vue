<template>
  <div class="container">
    <!-- 面包屑导航 -->    
    <div class="warp-breadcrum">
     <el-breadcrumb separator="/">
        <el-breadcrumb-item :to="{ path: '/' }"><b>首页</b></el-breadcrumb-item>
        <el-breadcrumb-item>基础数据</el-breadcrumb-item>
        <el-breadcrumb-item>奖项管理</el-breadcrumb-item>
    </el-breadcrumb>         
    </div>
    <!-- 下方主内容 -->       
    <div class="warp-body">  
      <!-- 工具栏 -->         
      <div class="toolbal">  
        <el-form :inline="true" style="margin-bottom:15px">
          <el-button type="primary" @click="addFormVisible = true">新增奖项</el-button>
        </el-form>           
      </div>
      <!-- 工具栏 -->         
      <div class="main-data">       
          <el-table  class="table" :data="AwdData" style="width:100%" v-loading="listLoading" height="string" :default-sort = "{prop: 'GradeName', order: 'descending'}" @selection-change="selRowChange" > 
            <el-table-column type="selection" width="55"></el-table-column>
            <el-table-column type="index" width="65" label="序号" align="center"></el-table-column>
            <el-table-column prop="Name" label="奖项名称" align="center" sortable></el-table-column>
            <el-table-column prop="GradeName" label="奖项级别"  align="center" :formatter="transfGrandeName" sortable></el-table-column>
            <el-table-column prop="Grade" label="获奖等次" align="center" :formatter="transfGrande" sortable></el-table-column>
            <el-table-column label="操作" width="150">
              <template slot-scope="scope">
                <el-button  size="small" @click="showModifyDialog(scope.$index, scope.row)">编辑</el-button>
                <el-button type="danger" size="small" @click="delectAwd(scope.$index,scope.row)" >删除</el-button>
              </template>
            </el-table-column>
          </el-table>          
      </div>
        <el-pagination layout="total, prev, pager, next, sizes, jumper" @size-change="SizeChangeEvent" @current-change="CurrentChangeEvent" :page-size="size" :page-sizes="[10,15,20,25,30]":total="totalNum" >
        </el-pagination>      
    </div>
      <!-- 新增表单 -->
    <el-dialog title="新增奖项" :visible.sync="addFormVisible" v-loading="submitLoading">
      <el-form :model="addFormBody" label-width="80px" ref="addForm" :rules="rules" auto>
        <el-form-item label="奖项名称" prop="Name">
          <el-input v-model="addFormBody.Name" placeholder="名称不含年份"  ></el-input>
        </el-form-item>
        <el-form-item label="奖项级别" prop="GradeName">
          <el-select v-model="addFormBody.GradeName" placeholder="请选择级别">
            <el-option v-for="GradeName in GradeNames" :key="GradeName.value" :label="GradeName.label" :value="GradeName.value"></el-option>
          </el-select>
        </el-form-item> 
        <el-form-item label="获奖等次" prop="Grade">
          <el-select v-model="addFormBody.Grade" placeholder="请选择等次">
            <el-option v-for="Grade in Grades" :key="Grade.value" :label="Grade.label" :value="Grade.value"></el-option>
          </el-select>
        </el-form-item>         
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native=" addFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="addSubmit" >提交</el-button>
      </div>
    </el-dialog>

    <!-- 编辑表单 -->
    <el-dialog title="编辑奖项" :visible.sync="modifyFormVisible" v-loading="modifyLoading">
      <el-form :model="modifyFromBody" label-width="80px" ref="modifyFrom" :rules="rules" >
        <el-form-item label="奖项名称" prop="Name" >
          <el-input v-model="modifyFromBody.Name" placeholder="名称不含年份"  ></el-input>
        </el-form-item>
        <el-form-item label="奖项名称" prop="GradeName">
          <el-select v-model="modifyFromBody.GradeName" placeholder="请选择级别">
            <el-option v-for="Grade in GradeNames" :key="Grade.value" :label="Grade.label" :value="Grade.value"></el-option>
          </el-select>
        </el-form-item>  
        <el-form-item label="获奖等次" prop="Grade">
          <el-select v-model="modifyFromBody.Grade" placeholder="请选择等次">
            <el-option v-for="Grade in Grades" :key="Grade.value" :label="Grade.label" :value="Grade.value"></el-option>
          </el-select>
        </el-form-item>    
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native=" modifyFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="modifySubmit" >提交</el-button>
      </div>     
    </el-dialog>        
  </div>
</template>

<script type="text/ecmascript-6">
import { GradeNames,Grades} from '../../assets/data/basic'
import {
  reqGetAwdList,
  posAddAwd,
  posModifyAwd,
  reqDeleteAwd
} from "../../api/api";
import PubMethod from "../../common/util";

export default {
  data() {
    return {
      //用户令牌
      access_token: "",
      //表格数据
      AwdData: [],
      totalNum: 0,
      page: 1,
      size: 10,
      listLoading: true,
      selection: [],

      //表单验证规则
      rules: {
        Name: {
          required: true,
          message: "请输入奖项名称",
          trigger: "blur"
        },
        GradeName: {
          required: true,
          message: "请选择级别",
          trigger: "change"
        },
        Grade: {
          required: true,
          message: "请选择等次",
          trigger: "change"
        }
      },

      //新增表单相关数据
      addFormVisible: false,
      submitLoading: false,
      //新增表单中奖项级别
      addFormBody: {
        Name: "",
        GradeName: "",
        Grade: ""
      },
      GradeNames,
      //新增表单中奖项等次
      Grades,
      //编辑表单相关数据
      modifyFormVisible: false,
      modifyLoading: false,
      modifyFromBody: {
        Name: "",
        GradeName: "",
        Grade: ""
      }
    };
  },
  mounted() {
    this.getList();
  },
  methods: {
    //公共类方法
    logInfo(message) {
      PubMethod.logMessage(message);
    },
    //奖项级别转换
    transfGrandeName(row) {
      return PubMethod.transfGrandeName(row);
    },
    //奖项级别转换
    transfGrande(row) {
      //console.log(row)
      return PubMethod.transfGrande(row);
    },
    // 当选择行时候
    selRowChange(selection) {
      this.selection = selection;
    },
    //获取荣誉列表
    getList() {
      this.listLoading = true;
      let param = {
        access_token: "11",
        page: this.page,
        limit: this.size
      };
      reqGetAwdList(param).then(res => {
        this.AwdData = res.data.data.list;
        this.totalNum = res.data.data.count;
        this.listLoading = false;
      }).catch *
        (res => {
          PubMethod.logMessage(res);
        });
    },
    //新增荣誉项
    addSubmit() {
      let param;
      this.$refs["addForm"].validate(valid => {
        if (valid) {
          this.submitLoading = true;
          //复制字符串
          let para = Object.assign({}, this.addFormBody);
          para.access_token = "terry";
          posAddAwd(para).then(res => {
            this.submitLoading = false;
            //公共提示方法，传入当前的vue以及res.data
            PubMethod.statusinfo(this, res.data);
            this.$refs["addForm"].resetFields();
            this.addFormVisible = false;
            this.getList();
          });
        }
      });
    },
    //显示编辑
    showModifyDialog(index, row) {
      this.modifyFormVisible = true;
      this.modifyFromBody = Object.assign({}, row);
      console.log(row);
      this.selectRowIndex = index;
      //console.log(this.selectRowIndex)
    },
    //保存编辑
    modifySubmit() {
      this.$refs["modifyFrom"].validate(valid => {
        if (valid) {
          this.modifyLoading = true;
          let para = Object.assign({}, this.modifyFromBody);
          para.access_token = "terry";

          posModifyAwd(para).then(res => {
            //公共提示方法，传入当前的vue以及res.data
            PubMethod.statusinfo(this, res.data);
            this.$refs["modifyFrom"].resetFields();
            this.modifyFormVisible = false;
            this.modifyLoading = false;
            this.getList();
          });
        }
      });
    },
    //删除功能
    delectAwd(index, row) {
      this.$confirm("此操作将永久删除该奖项, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          let para = { awardID: row.AwdID };
          para.access_token = "terry";

          reqDeleteAwd(para).then(res => {
            //公共提示方法，传入当前的vue以及res.data
            PubMethod.statusinfo(this, res.data);
            this.getList();
          });
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "已取消删除"
          });
        });
    },
    // 删除选中项目
    delectSelAwd() {
      let awardIDs = this.selection.map(singleSel => singleSel.AwdID);
      this.$confirm("此操作将永久删除该奖项, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          awardIDs.map(awardID => {
            let para = { awardID };
            para.access_token = "terry";
            reqDeleteAwd(para).then(res => {
              //公共提示方法，传入当前的vue以及res.data
              PubMethod.statusinfo(this, res.data);
            });
            this.getList();
          });
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "已取消删除"
          });
        });
      // this.$message({
      //   type:'error',
      //   message:'暂不支持批量删除'
      // })
    },
    //更换每页数量
    SizeChangeEvent(val) {
      this.size = val;
      this.getList();
      PubMethod.logMessage(this.page + "   " + this.size);
    },
    //页码切换时
    CurrentChangeEvent(val) {
      this.page = val;
      this.getList();
      PubMethod.logMessage(this.page + "   " + this.size);
    }
  }
};
</script>

<style scoped lang="scss">
.el-pagination {
  text-align: right;
}
</style>
