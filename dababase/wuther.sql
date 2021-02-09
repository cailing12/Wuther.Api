/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50713
Source Host           : 127.0.0.1:3306
Source Database       : wuther

Target Server Type    : MYSQL
Target Server Version : 50713
File Encoding         : 65001

Date: 2021-02-09 20:49:08
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for blogs
-- ----------------------------
DROP TABLE IF EXISTS `blogs`;
CREATE TABLE `blogs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(100) NOT NULL,
  `Abstract` varchar(300) DEFAULT NULL,
  `UserId` int(11) NOT NULL,
  `MenuId` int(11) NOT NULL,
  `Like` int(11) DEFAULT NULL,
  `Comment` int(11) DEFAULT NULL,
  `Trend` int(11) DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  `ModifyTime` datetime DEFAULT NULL,
  `Path` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `blog_user` (`UserId`),
  KEY `blog_menu` (`MenuId`),
  CONSTRAINT `blog_menu` FOREIGN KEY (`MenuId`) REFERENCES `menus` (`Id`),
  CONSTRAINT `blog_user` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of blogs
-- ----------------------------
INSERT INTO `blogs` VALUES ('1', '从哲学层面浅谈计算机学习方法论', '之前思考和总结过两篇关于学习的文章，（《如何快速且深入的学习一门新技术》，《微服务学习导航》），个人感觉还是言不尽兴，太过肤浅了。所以这篇文章会从更高的形而上的角度来审视自己的学习。其中的思想来源比较复杂，主要是受了老子、王东岳、李善长、古典文学和计算机科学等的影响，不知其所踪。 学什么？ 抽象模型 .', '1', '8', '10', '20', '5', '2021-01-21 19:57:25', '2021-01-21 19:57:40', null);
INSERT INTO `blogs` VALUES ('2', 'ASP Net Core – CORS 预检请求', '对于某些 CORS 请求，浏览器会在发出实际请求之前发送额外的 OPTIONS 请求。 此请求称为 预检请求。 如果满足以下 所有 条件，浏览器可以跳过预检请求： 请求方法为 GET、HEAD 或 POST。应用不会设置、、、或以外的请求标头 Accept Accept-Language Conte ...', '1', '3', '20', '30', '1', '2021-01-25 20:56:58', '2021-01-25 20:57:03', null);
INSERT INTO `blogs` VALUES ('6', 'Vue相关事件', null, '1', '4', null, null, null, '2021-02-09 17:30:09', null, '\\admin\\p\\16128630.html');

-- ----------------------------
-- Table structure for menus
-- ----------------------------
DROP TABLE IF EXISTS `menus`;
CREATE TABLE `menus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) DEFAULT NULL,
  `Position` int(1) DEFAULT '0' COMMENT '0:横栏，1：竖栏',
  `ParentId` int(11) DEFAULT NULL,
  `Icon` varchar(20) DEFAULT NULL,
  `Path` varchar(100) DEFAULT NULL,
  `Seqno` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of menus
-- ----------------------------
INSERT INTO `menus` VALUES ('1', '首页', '0', null, '&#ed67f', '/home', '1');
INSERT INTO `menus` VALUES ('2', '关于', '0', null, '&#xe68f', '/about', '2');
INSERT INTO `menus` VALUES ('3', '.Net', '1', null, null, '/net', '1');
INSERT INTO `menus` VALUES ('4', '前端', '1', null, null, '/js', '2');
INSERT INTO `menus` VALUES ('5', '数据库', '1', null, null, '/sql', '3');
INSERT INTO `menus` VALUES ('6', 'oracle', '1', '5', null, '/oralce', '1');
INSERT INTO `menus` VALUES ('7', 'mysql', '1', '5', null, '/mysql', '2');
INSERT INTO `menus` VALUES ('8', '其他', '1', null, null, '/others', '4');
INSERT INTO `menus` VALUES ('9', '新博客', '0', null, null, '/blog', '3');

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Account` varchar(15) NOT NULL,
  `Username` varchar(20) DEFAULT NULL,
  `Password` varchar(20) DEFAULT NULL,
  `Sex` int(1) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Department` varchar(20) DEFAULT NULL,
  `Phone` varchar(15) DEFAULT NULL,
  `WrittenOffTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES ('1', 'admin', '神', 'admin123', '1', 'wuther@xx.com', 'IT', '13312345678', null);
INSERT INTO `users` VALUES ('2', 'natsume', '系统管理员', null, '1', 'natsume@xx.com', 'IT', '13312345678', '2020-09-24 22:01:00');
