package com.example.backend.achivement.mapper;

import com.example.backend.model.AchivementModel;
import org.apache.ibatis.annotations.Mapper;

@Mapper
public interface AchivementMapper {

    public AchivementModel getAchivement(AchivementModel achivementModel);
}
