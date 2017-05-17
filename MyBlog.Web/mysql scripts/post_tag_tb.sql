/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50717
Source Host           : 127.0.0.1:3306
Source Database       : my_blog_db

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2017-05-16 09:55:00
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for post_tag_tb
-- ----------------------------
DROP TABLE IF EXISTS `post_tag_tb`;
CREATE TABLE `post_tag_tb` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `post_id` varchar(255) NOT NULL,
  `tag_name` varchar(255) NOT NULL,
  `pub_state` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `tag_name_fk` (`tag_name`),
  KEY `post_id_fk` (`post_id`),
  KEY `pub_state_fk` (`pub_state`),
  CONSTRAINT `post_id_fk` FOREIGN KEY (`post_id`) REFERENCES `post_tb` (`post_id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `pub_state_fk` FOREIGN KEY (`pub_state`) REFERENCES `post_tb` (`post_pub_state`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;
