package com.example.backend.achivement.mapper;

import com.example.backend.model.AchivementModel;
import com.example.backend.model.UserModel;
import org.apache.ibatis.annotations.Mapper;

@Mapper
public interface achivementMapper {

    public AchivementModel getAchivement(AchivementModel achivementModel);
}
