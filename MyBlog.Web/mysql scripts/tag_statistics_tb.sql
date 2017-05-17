/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50717
Source Host           : 127.0.0.1:3306
Source Database       : my_blog_db

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2017-05-16 09:55:12
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for tag_statistics_tb
-- ----------------------------
DROP TABLE IF EXISTS `tag_statistics_tb`;
CREATE TABLE `tag_statistics_tb` (
  `tag_name` varchar(255) NOT NULL,
  `tag_count` int(255) DEFAULT NULL,
  PRIMARY KEY (`tag_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
