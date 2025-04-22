package com.example.backend.dbconfig;

import org.apache.ibatis.session.SqlSessionFactory;
import org.mybatis.spring.SqlSessionFactoryBean;
import org.mybatis.spring.annotation.MapperScan;
import org.mybatis.spring.boot.autoconfigure.SpringBootVFS;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.support.PathMatchingResourcePatternResolver;

import javax.sql.DataSource;

@Configuration
@MapperScan(value = "com.example.backend", sqlSessionFactoryRef = "dbSqlSessionFactory")
public class DatabaseConfig {

    @Bean(name = "dbSqlSessionFactory")
    public SqlSessionFactory dbSqlSessionFactory(DataSource dbDataSource) throws Exception {

        SqlSessionFactoryBean sqlSessionFactoryBean = new SqlSessionFactoryBean();
        sqlSessionFactoryBean.setVfs(SpringBootVFS.class);
        sqlSessionFactoryBean.setDataSource(dbDataSource);

        PathMatchingResourcePatternResolver resolver = new PathMatchingResourcePatternResolver();
        sqlSessionFactoryBean.setTypeAliasesPackage("com.example.backend.model");
        sqlSessionFactoryBean.setMapperLocations(resolver.getResources("classpath*:com/example/backend/**/*-Sql.xml"));

        org.apache.ibatis.session.Configuration mybatisconfig = new org.apache.ibatis.session.Configuration();
        mybatisconfig.setMapUnderscoreToCamelCase(true);
        sqlSessionFactoryBean.setConfiguration(mybatisconfig);

        return  sqlSessionFactoryBean.getObject();
    }
}
